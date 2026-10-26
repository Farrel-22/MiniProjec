document.addEventListener('DOMContentLoaded', async () => {
    try {
        await MiniApp.requireAuth();
    } catch (e) {
        // Continue even if login check fails
    }
    loadKotas();
});

async function loadKotas() {
    try {
        const items = await MiniApp.api('GET', '/api/kota');
        const tbody = document.getElementById('kotaTableBody');

        if (!items || items.length === 0) {
            tbody.innerHTML = '<tr><td colspan="4" class="text-center text-muted">Belum ada data kota.</td></tr>';
            return;
        }

        tbody.innerHTML = items.map((item, index) => `
            <tr>
                <td>${index + 1}</td>
                <td><span class="badge bg-secondary">${item.id}</span></td>
                <td>${MiniApp.escapeHtml(item.nama)}</td>
                <td>
                    <a class="btn btn-sm btn-warning" href="/kota/form.html?id=${item.id}">Edit</a>
                    <button type="button" class="btn btn-sm btn-outline-danger btn-delete" data-id="${item.id}">Hapus</button>
                </td>
            </tr>`).join('');
    } catch (err) {
        MiniApp.showAlert('danger', 'Gagal memuat data kota.');
    }
}

document.getElementById('kotaTableBody').addEventListener('click', async (e) => {
    const btn = e.target.closest('.btn-delete');
    if (!btn) return;

    if (!confirm('Apakah Anda yakin ingin menghapus kota ini?')) return;

    const id = btn.dataset.id;
    try {
        const res = await MiniApp.api('DELETE', '/api/kota/' + id);
        MiniApp.showAlert('success', res.message);
        loadKotas();
    } catch (err) {
        MiniApp.showAlert('danger', (err && err.data && err.data.message) || 'Gagal menghapus data.');
    }
});
