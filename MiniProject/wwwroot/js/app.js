(function () {
    'use strict';

    const FLASH_KEY = 'miniapp.flash';
    const MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    function pad2(n) {
        return n < 10 ? '0' + n : String(n);
    }

    async function api(method, url, body) {
        const hasBody = body !== undefined;
        const res = await fetch(url, {
            method: method,
            credentials: 'same-origin',
            headers: Object.assign(
                { 'Accept': 'application/json' },
                hasBody ? { 'Content-Type': 'application/json' } : {}
            ),
            body: hasBody ? JSON.stringify(body) : undefined
        });

        const contentType = res.headers.get('content-type') || '';
        let data = null;
        if (contentType.indexOf('json') !== -1) {
            data = await res.json().catch(() => null);
        }

        if (res.status === 401 && location.pathname !== '/login.html') {
            location.href = '/login.html?returnUrl=' + encodeURIComponent(location.pathname + location.search);
        }

        if (!res.ok) {
            throw { status: res.status, data: data };
        }

        return data;
    }

    function escapeHtml(value) {
        if (value === null || value === undefined) {
            return '';
        }
        return String(value)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    function renderNavbar(user) {
        const container = document.getElementById('appNavbar');
        if (!container) {
            return;
        }

        const displayName = escapeHtml(user.fullName || user.username);

        container.innerHTML = `
<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
    <div class="container">
        <a class="navbar-brand" href="/index.html">MiniProject</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#mainNavbar" aria-controls="mainNavbar" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="mainNavbar">
            <ul class="navbar-nav me-auto">
                <li class="nav-item"><a class="nav-link" href="/index.html">Home</a></li>
                <li class="nav-item dropdown">
                    <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">Master</a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="/region/index.html">Region</a></li>
                        <li><a class="dropdown-item" href="/branch/index.html">Branch</a></li>
                    </ul>
                </li>
                <li class="nav-item dropdown">
                    <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">Transaction</a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="/budget/index.html">Budget</a></li>
                    </ul>
                </li>
            </ul>
            <div class="d-flex align-items-center gap-3">
                <span class="navbar-text text-light">${displayName}</span>
                <button type="button" id="btnLogout" class="btn btn-outline-light btn-sm">Logout</button>
            </div>
        </div>
    </div>
</nav>`;

        const btnLogout = document.getElementById('btnLogout');
        if (btnLogout) {
            btnLogout.addEventListener('click', function () {
                api('POST', '/api/auth/logout').finally(() => {
                    location.href = '/login.html';
                });
            });
        }
    }

    function showAlert(type, message) {
        const container = document.getElementById('appAlert');
        if (!container) {
            return;
        }
        container.innerHTML = `<div class="alert alert-${type} alert-dismissible fade show" role="alert">${escapeHtml(message)}<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>`;
    }

    function setFlash(type, message) {
        sessionStorage.setItem(FLASH_KEY, JSON.stringify({ type: type, message: message }));
    }

    function showFlash() {
        const raw = sessionStorage.getItem(FLASH_KEY);
        if (!raw) {
            return;
        }
        sessionStorage.removeItem(FLASH_KEY);

        let flash = null;
        try {
            flash = JSON.parse(raw);
        } catch (e) {
            flash = null;
        }

        if (flash) {
            showAlert(flash.type, flash.message);
        }
    }

    async function requireAuth() {
        const user = await api('GET', '/api/auth/me');
        renderNavbar(user);
        showFlash();
        return user;
    }

    function clearErrors(form) {
        form.querySelectorAll('[data-field]').forEach(function (el) {
            el.textContent = '';
        });
        form.querySelectorAll('[data-summary]').forEach(function (el) {
            el.textContent = '';
        });
    }

    function showErrors(form, err) {
        const data = err && err.data;
        const summary = form.querySelector('[data-summary]');
        const extra = [];
        let shown = false;

        if (data && data.errors) {
            const fields = Array.from(form.querySelectorAll('[data-field]'));
            Object.keys(data.errors).forEach(function (key) {
                const messages = data.errors[key];
                const list = Array.isArray(messages) ? messages : [messages];
                const normalized = key.replace(/^\$\./, '').toLowerCase();

                const target = fields.find(function (el) {
                    return el.dataset.field.toLowerCase() === normalized;
                });

                if (target) {
                    target.textContent = list.join(' ');
                    shown = true;
                } else {
                    list.forEach(function (m) {
                        extra.push(m);
                    });
                }
            });
        }

        if (data && data.message) {
            extra.push(data.message);
        }

        if (!shown && extra.length === 0) {
            extra.push('An unexpected error occurred.');
        }

        if (summary) {
            summary.textContent = extra.join(' ');
        }
    }

    function getQueryParam(name) {
        return new URLSearchParams(location.search).get(name);
    }

    function formatNumber(value) {
        return Number(value).toLocaleString('en-US', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });
    }

    function formatDateTime(value) {
        if (!value) {
            return '';
        }
        const date = new Date(value);
        if (isNaN(date.getTime())) {
            return '';
        }
        return pad2(date.getDate()) + ' ' + MONTHS[date.getMonth()] + ' ' + date.getFullYear() + ' ' +
            pad2(date.getHours()) + ':' + pad2(date.getMinutes());
    }

    function toNullable(value) {
        if (value == null) {
            return null;
        }
        const trimmed = String(value).trim();
        return trimmed === '' ? null : trimmed;
    }

    window.MiniApp = {
        api: api,
        requireAuth: requireAuth,
        showAlert: showAlert,
        setFlash: setFlash,
        showFlash: showFlash,
        clearErrors: clearErrors,
        showErrors: showErrors,
        getQueryParam: getQueryParam,
        escapeHtml: escapeHtml,
        formatNumber: formatNumber,
        formatDateTime: formatDateTime,
        toNullable: toNullable
    };
})();
