document.addEventListener('DOMContentLoaded', async () => {
    await MiniApp.requireAuth();
    loadRegions();
});

async function loadRegions() {
    try {
        const items = await MiniApp.api('GET', '/api/region');
        const tbody = document.getElementById('regionTableBody');

        if (!items || items.length === 0) {
            tbody.innerHTML = '<tr><td colspan="5" class="text-center">No data.</td></tr>';
            return;
        }

        tbody.innerHTML = items.map((item, index) => `
            <tr>
                <td>${index + 1}</td>
                <td>${MiniApp.escapeHtml(item.txtCode)}</td>
                <td>${MiniApp.escapeHtml(item.txtName)}</td>
                <td>${MiniApp.formatDateTime(item.dtCreated)}</td>
                <td>
                    <a class="btn btn-sm btn-warning" href="/region/form.html?id=${item.uid}">Edit</a>
                    <button type="button" class="btn btn-sm btn-outline-danger btn-delete" data-id="${item.uid}">Delete</button>
                </td>
            </tr>`).join('');
    } catch (err) {
        MiniApp.showAlert('danger', 'Failed to load data.');
    }
}

document.getElementById('regionTableBody').addEventListener('click', async (e) => {
    const btn = e.target.closest('.btn-delete');
    if (!btn) return;

    if (!confirm('Delete this region?')) return;

    const id = btn.dataset.id;
    try {
        const res = await MiniApp.api('DELETE', '/api/region/' + id);
        MiniApp.showAlert('success', res.message);
        loadRegions();
    } catch (err) {
        MiniApp.showAlert('danger', (err && err.data && err.data.message) || 'Delete failed.');
    }
});
