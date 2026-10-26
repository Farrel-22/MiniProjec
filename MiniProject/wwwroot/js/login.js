document.addEventListener('DOMContentLoaded', () => {
    MiniApp.api('GET', '/api/auth/me')
        .then(() => location.href = '/index.html')
        .catch(() => {});

    const form = document.getElementById('loginForm');
    const submitButton = form.querySelector('button[type="submit"]');

    form.addEventListener('submit', async (event) => {
        event.preventDefault();
        MiniApp.clearErrors(form);

        const body = {
            username: document.getElementById('Username').value,
            password: document.getElementById('Password').value
        };

        submitButton.disabled = true;
        try {
            await MiniApp.api('POST', '/api/auth/login', body);
            const returnUrl = MiniApp.getQueryParam('returnUrl');
            location.href = (returnUrl && returnUrl.startsWith('/') && !returnUrl.startsWith('//'))
                ? returnUrl
                : '/index.html';
        } catch (err) {
            MiniApp.showErrors(form, err);
        } finally {
            submitButton.disabled = false;
        }
    });
});
