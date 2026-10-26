document.addEventListener('DOMContentLoaded', async () => {
    await MiniApp.requireAuth();

    const id = MiniApp.getQueryParam('id');
    const form = document.getElementById('regionForm');
    const submitBtn = form.querySelector('[type="submit"]');
    const TxtCode = document.getElementById('TxtCode');
    const TxtName = document.getElementById('TxtName');

    if (id) {
        document.getElementById('formTitle').textContent = 'Edit Region';
        document.title = 'Edit Region - MiniProject';

        try {
            const item = await MiniApp.api('GET', '/api/region/' + id);
            document.getElementById('Uid').value = item.uid;
            TxtCode.value = item.txtCode;
            TxtName.value = item.txtName;
        } catch (err) {
            MiniApp.showAlert('danger', (err.data && err.data.message) || 'Region not found.');
            return;
        }
    } else {
        document.getElementById('formTitle').textContent = 'Add Region';
        document.title = 'Add Region - MiniProject';
    }

    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        MiniApp.clearErrors(form);

        const body = {
            uid: id || null,
            txtCode: TxtCode.value.trim(),
            txtName: TxtName.value.trim()
        };

        submitBtn.disabled = true;
        try {
            const res = id
                ? await MiniApp.api('PUT', '/api/region/' + id, body)
                : await MiniApp.api('POST', '/api/region', body);
            MiniApp.setFlash('success', res.message);
            location.href = '/region/index.html';
        } catch (err) {
            MiniApp.showErrors(form, err);
        } finally {
            submitBtn.disabled = false;
        }
    });
});
