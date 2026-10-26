document.addEventListener('DOMContentLoaded', async () => {
    await MiniApp.requireAuth();
    loadBudgets();

    document.getElementById('budgetTableBody').addEventListener('click', async (e) => {
        const btn = e.target.closest('.btn-delete');
        if (!btn) return;

        if (!confirm('Delete this budget?')) return;

        try {
            const res = await MiniApp.api('DELETE', '/api/budget/' + btn.dataset.id);
            MiniApp.showAlert('success', res.message);
            loadBudgets();
        } catch (err) {
            MiniApp.showAlert('danger', (err && err.data && err.data.message) || 'Delete failed.');
        }
    });
});

async function loadBudgets() {
    const tbody = document.getElementById('budgetTableBody');

    try {
        const items = await MiniApp.api('GET', '/api/budget');

        if (!items || items.length === 0) {
            tbody.innerHTML = '<tr><td colspan="7" class="text-center">No data.</td></tr>';
            return;
        }

        tbody.innerHTML = items.map((item, index) => `
            <tr>
                <td>${index + 1}</td>
                <td>${MiniApp.escapeHtml(item.regionName)}</td>
                <td>${MiniApp.escapeHtml(item.branchName)}</td>
                <td>${item.intYear}</td>
                <td class="text-end">${MiniApp.formatNumber(item.decAmount)}</td>
                <td>${MiniApp.escapeHtml(item.txtDescription)}</td>
                <td>
                    <a href="/budget/form.html?id=${item.uid}" class="btn btn-sm btn-outline-secondary">Edit</a>
                    <button type="button" class="btn btn-sm btn-outline-danger btn-delete" data-id="${item.uid}">Delete</button>
                </td>
            </tr>
        `).join('');
    } catch (err) {
        MiniApp.showAlert('danger', 'Failed to load data.');
    }
}
