document.addEventListener('DOMContentLoaded', async () => {
    await MiniApp.requireAuth();

    const id = MiniApp.getQueryParam('id');
    const form = document.getElementById('budgetForm');
    const submitBtn = form.querySelector('[type="submit"]');
    const Uid = document.getElementById('Uid');
    const RefRegionId = document.getElementById('RefRegionId');
    const RefBranchId = document.getElementById('RefBranchId');
    const IntYear = document.getElementById('IntYear');
    const DecAmount = document.getElementById('DecAmount');
    const TxtDescription = document.getElementById('TxtDescription');
    const formTitle = document.getElementById('formTitle');

    const regions = await MiniApp.api('GET', '/api/region');
    regions.forEach(r => {
        const opt = document.createElement('option');
        opt.value = r.uid;
        opt.textContent = r.txtName;
        RefRegionId.appendChild(opt);
    });

    async function loadBranches(regionUid, selectedUid) {
        RefBranchId.innerHTML = '';
        const first = document.createElement('option');
        first.value = '';
        first.textContent = '-- Select Branch --';
        RefBranchId.appendChild(first);

        if (!regionUid) return;

        const branches = await MiniApp.api('GET', '/api/branch/by-region/' + regionUid);
        branches.forEach(b => {
            const opt = document.createElement('option');
            opt.value = b.uid;
            opt.textContent = b.txtCode + ' - ' + b.txtName;
            RefBranchId.appendChild(opt);
        });

        if (selectedUid) {
            RefBranchId.value = selectedUid;
        }
    }

    RefRegionId.addEventListener('change', function () {
        loadBranches(this.value, null);
    });

    if (id) {
        formTitle.textContent = 'Edit Budget';
        document.title = 'Edit Budget - MiniProject';
        try {
            const item = await MiniApp.api('GET', '/api/budget/' + id);
            Uid.value = item.uid;
            RefRegionId.value = item.refRegionId || '';
            await loadBranches(item.refRegionId, item.refBranchId);
            IntYear.value = item.intYear;
            DecAmount.value = item.decAmount;
            TxtDescription.value = item.txtDescription || '';
        } catch (err) {
            MiniApp.showAlert('danger', (err.data && err.data.message) || 'Budget not found.');
            return;
        }
    } else {
        formTitle.textContent = 'Add Budget';
        document.title = 'Add Budget - MiniProject';
        IntYear.value = new Date().getFullYear();
    }

    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        MiniApp.clearErrors(form);

        const body = {
            uid: id || null,
            refRegionId: MiniApp.toNullable(RefRegionId.value),
            refBranchId: MiniApp.toNullable(RefBranchId.value),
            intYear: IntYear.value === '' ? null : parseInt(IntYear.value, 10),
            decAmount: DecAmount.value === '' ? null : parseFloat(DecAmount.value),
            txtDescription: MiniApp.toNullable(TxtDescription.value)
        };

        submitBtn.disabled = true;
        try {
            const res = id
                ? await MiniApp.api('PUT', '/api/budget/' + id, body)
                : await MiniApp.api('POST', '/api/budget', body);
            MiniApp.setFlash('success', res.message);
            location.href = '/budget/index.html';
        } catch (err) {
            MiniApp.showErrors(form, err);
            submitBtn.disabled = false;
        }
    });
});
