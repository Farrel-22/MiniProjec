document.addEventListener('DOMContentLoaded', async () => {
    try {
        await MiniApp.requireAuth();
    } catch (e) {
        // Continue even if login check fails
    }

    const id = MiniApp.getQueryParam('id');
    const form = document.getElementById('kotaForm');
    const submitBtn = form.querySelector('[type="submit"]');
    const namaInput = document.getElementById('Nama');

    if (id) {
        document.getElementById('formTitle').textContent = 'Edit Kota';
        document.title = 'Edit Kota - MiniProject';

        try {
            const item = await MiniApp.api('GET', '/api/kota/' + id);
            document.getElementById('Id').value = item.id;
            namaInput.value = item.nama;
        } catch (err) {
            MiniApp.showAlert('danger', (err.data && err.data.message) || 'Data kota tidak ditemukan.');
            return;
        }
    } else {
        document.getElementById('formTitle').textContent = 'Tambah Kota';
        document.title = 'Tambah Kota - MiniProject';
    }

    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        MiniApp.clearErrors(form);

        const body = {
            id: id ? parseInt(id) : null,
            nama: namaInput.value.trim()
        };

        submitBtn.disabled = true;
        try {
            const res = id
                ? await MiniApp.api('PUT', '/api/kota/' + id, body)
                : await MiniApp.api('POST', '/api/kota', body);
            MiniApp.setFlash('success', res.message);
            location.href = '/kota/index.html';
        } catch (err) {
            MiniApp.showErrors(form, err);
        } finally {
            submitBtn.disabled = false;
        }
    });
});
