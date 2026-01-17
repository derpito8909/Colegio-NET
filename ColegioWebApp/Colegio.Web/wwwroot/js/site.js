document.addEventListener('DOMContentLoaded', () => {    
    const actionModal = document.getElementById('confirmActionModal');
    if (!actionModal) return;

    let targetForm = null;

    actionModal.addEventListener('show.bs.modal', (event) => {
        const btn = event.relatedTarget;
        if (!btn) return;

        const formSelector = btn.getAttribute('data-form');
        const title = btn.getAttribute('data-title') ?? 'Confirmar';
        const text = btn.getAttribute('data-text') ?? '¿Confirmas la acción?';

        const titleEl = document.getElementById('confirmActionTitle');
        const textEl = document.getElementById('confirmActionText');

        if (titleEl) titleEl.textContent = title;
        if (textEl) textEl.textContent = text;

        targetForm = formSelector ? document.querySelector(formSelector) : null;
    });

    const confirmBtn = document.getElementById('confirmActionBtn');
    if (confirmBtn) {
        confirmBtn.addEventListener('click', () => {
            if (targetForm) targetForm.submit();
        });
    }
});


document.addEventListener('DOMContentLoaded', () => {
    const modal = document.getElementById('confirmDeleteModal');
    if (!modal) return; // si no existe, no hace nada

    modal.addEventListener('show.bs.modal', (event) => {
        const btn = event.relatedTarget;
        if (!btn) return;

        const id = btn.getAttribute('data-id') ?? '';
        const page = btn.getAttribute('data-page') ?? '1';
        const pageSize = btn.getAttribute('data-pagesize') ?? '10';
        const text = btn.getAttribute('data-text') ?? '¿Seguro que deseas eliminar este registro?';

        const title = btn.getAttribute('data-title');
        const titleEl = document.getElementById('confirmDeleteTitle');
        if (titleEl && title) titleEl.textContent = title;
        const idInput = document.getElementById('confirmDeleteId');
        const pageInput = document.getElementById('confirmDeletePage');
        const pageSizeInput = document.getElementById('confirmDeletePageSize');
        const textEl = document.getElementById('confirmDeleteText');

        if (idInput) idInput.value = id;
        if (pageInput) pageInput.value = page;
        if (pageSizeInput) pageSizeInput.value = pageSize;
        if (textEl) textEl.textContent = text;
    });
});

