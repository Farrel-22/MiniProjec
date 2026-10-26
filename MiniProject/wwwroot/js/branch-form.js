document.addEventListener('DOMContentLoaded', async () => {
    await MiniApp.requireAuth();

    const id = MiniApp.getQueryParam('id');
    const form = document.getElementById('branchForm');
    const formTitle = document.getElementById('formTitle');
    const uidInput = document.getElementById('Uid');
    const refRegionIdSelect = document.getElementById('RefRegionId');
    const txtCodeInput = document.getElementById('TxtCode');
    const txtNameInput = document.getElementById('TxtName');
    const txtAddressInput = document.getElementById('TxtAddress');
    const submitBtn = form.querySelector('button[type="submit"]');

    const regions = await MiniApp.api('GET', '/api/region');
    regions.forEach(region => {
        const option = document.createElement('option');
        option.value = region.uid;
        option.textContent = region.txtName;
        refRegionIdSelect.appendChild(option);
    });

    if (id) {
        formTitle.textContent = 'Edit Branch';
        document.title = 'Edit Branch - MiniProject';

        let item;
        try {
            item = await MiniApp.api('GET', '/api/branch/' + id);
        } catch (err) {
            MiniApp.showAlert('danger', (err.data && err.data.message) || 'Branch not found.');
            return;
        }

        uidInput.value = item.uid;
        refRegionIdSelect.value = item.refRegionId;
        txtCodeInput.value = item.txtCode;
        txtNameInput.value = item.txtName;
        txtAddressInput.value = item.txtAddress || '';
    } else {
        formTitle.textContent = 'Add Branch';
        document.title = 'Add Branch - MiniProject';
    }

    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        MiniApp.clearErrors(form);
        submitBtn.disabled = true;

        const body = {
            uid: id || null,
            refRegionId: MiniApp.toNullable(refRegionIdSelect.value),
            txtCode: txtCodeInput.value.trim(),
            txtName: txtNameInput.value.trim(),
            txtAddress: MiniApp.toNullable(txtAddressInput.value)
        };

        try {
            const res = id
                ? await MiniApp.api('PUT', '/api/branch/' + id, body)
                : await MiniApp.api('POST', '/api/branch', body);
            MiniApp.setFlash('success', res.message);
            location.href = '/branch/index.html';
        } catch (err) {
            MiniApp.showErrors(form, err);
            submitBtn.disabled = false;
        }
    });
});
