document.addEventListener('DOMContentLoaded', async () => {
    await MiniApp.requireAuth();
    loadBranches();
});

document.getElementById('branchTableBody').addEventListener('click', onDeleteClick);

async function loadBranches() {
    const tbody = document.getElementById('branchTableBody');
    let branches;

    try {
        branches = await MiniApp.api('GET', '/api/branch');
    } catch (err) {
        MiniApp.showAlert('danger', 'Failed to load data.');
        return;
    }

    const list = branches || [];

    if (list.length === 0) {
        tbody.innerHTML = '<tr><td colspan="6" class="text-center">No data.</td></tr>';
        return;
    }

    tbody.innerHTML = list.map((item, index) => {
        const uid = item.uid;
        return `
            <tr>
                <td>${index + 1}</td>
                <td>${MiniApp.escapeHtml(item.regionName)}</td>
                <td>${MiniApp.escapeHtml(item.txtCode)}</td>
                <td>${MiniApp.escapeHtml(item.txtName)}</td>
                <td>${MiniApp.escapeHtml(item.txtAddress)}</td>
                <td>
                    <a href="/branch/form.html?id=${uid}" class="btn btn-sm btn-outline-primary">Edit</a>
                    <button type="button" class="btn btn-sm btn-outline-danger btn-delete" data-id="${uid}">Delete</button>
                </td>
            </tr>`;
    }).join('');
}

async function onDeleteClick(e) {
    const btn = e.target.closest('.btn-delete');
    if (!btn) return;

    if (!confirm('Delete this branch?')) return;

    const id = btn.dataset.id;

    try {
        const res = await MiniApp.api('DELETE', '/api/branch/' + id);
        MiniApp.showAlert('success', res.message);
        loadBranches();
    } catch (err) {
        MiniApp.showAlert('danger', (err && err.data && err.data.message) || 'Delete failed.');
    }
}
