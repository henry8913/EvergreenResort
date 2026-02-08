let modalCamera;

document.addEventListener('DOMContentLoaded', () => {
    modalCamera = new bootstrap.Modal(document.getElementById('modalCamera'));
    caricaDatiAdmin();
});

async function caricaDatiAdmin() {
    try {
        const response = await fetch('/api/camere');
        const camere = await response.json();
        const tbody = document.getElementById('tabella-admin-body');

        tbody.innerHTML = camere.map(c => `
            <tr>
                <td class="fw-bold">${c.numero}</td>
                <td>
                    <div class="d-flex align-items-center">
                        <img src="${c.immagineUrl || 'https://via.placeholder.com/50'}" class="rounded me-3" style="width: 50px; height: 50px; object-fit: cover;">
                        <div>
                            <div class="fw-bold">${c.nome}</div>
                            <small class="text-muted">${c.descrizione ? c.descrizione.substring(0, 40) + '...' : ''}</small>
                        </div>
                    </div>
                </td>
                <td><span class="badge bg-info text-dark">${c.tipologia || 'Standard'}</span></td>
                <td>€${c.prezzo.toFixed(2)}</td>
                <td>
                    <span class="badge status-badge ${getBadgeClass(c.stato)}">${c.stato}</span>
                </td>
                <td class="text-center">
                    <div class="btn-group" role="group">
                        <button class="btn btn-sm btn-outline-success" onclick="cambiaStato(${c.id}, 'Libera')" title="Segna come Libera"><i class="bi bi-check-circle"></i></button>
                        <button class="btn btn-sm btn-outline-danger" onclick="cambiaStato(${c.id}, 'Occupata')" title="Segna come Occupata"><i class="bi bi-person-fill-lock"></i></button>
                        <button class="btn btn-sm btn-outline-warning" onclick="cambiaStato(${c.id}, 'In Pulizia')" title="Segna come In Pulizia"><i class="bi bi-bucket"></i></button>
                         <button class="btn btn-sm btn-outline-secondary" onclick="cambiaStato(${c.id}, 'Manutenzione')" title="Segna in Manutenzione"><i class="bi bi-tools"></i></button>
                    </div>
                </td>
                <td class="text-center">
                     <button class="btn btn-sm btn-primary me-1" onclick="apriModal(${c.id})"><i class="bi bi-pencil"></i></button>
                     <button class="btn btn-sm btn-danger" onclick="eliminaCamera(${c.id})"><i class="bi bi-trash"></i></button>
                </td>
            </tr>
        `).join('');
    } catch (error) {
        console.error('Errore caricamento dati:', error);
    }
}

function getBadgeClass(stato) {
    if (stato === 'Libera') return 'bg-success';
    if (stato === 'Occupata') return 'bg-danger';
    if (stato === 'In Pulizia') return 'bg-warning text-dark';
    if (stato === 'Manutenzione') return 'bg-secondary';
    return 'bg-light text-dark';
}

async function cambiaStato(id, nuovoStato) {
    try {
        const response = await fetch(`/api/camere/cambia-stato/${id}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(nuovoStato)
        });

        if (response.ok) {
            caricaDatiAdmin();
        } else {
            alert('Errore nel cambio stato');
        }
    } catch (error) {
        console.error('Errore:', error);
    }
}

async function apriModal(id = null) {
    const form = document.getElementById('formCamera');
    form.reset();
    document.getElementById('cameraId').value = '';
    document.getElementById('modalTitle').innerText = 'Nuova Camera';

    if (id) {
        document.getElementById('modalTitle').innerText = 'Modifica Camera';
        try {
            const response = await fetch(`/api/camere/${id}`);
            const camera = await response.json();
            
            document.getElementById('cameraId').value = camera.id;
            document.getElementById('cameraNome').value = camera.nome;
            document.getElementById('cameraNumero').value = camera.numero;
            document.getElementById('cameraPrezzo').value = camera.prezzo;
            document.getElementById('cameraTipologia').value = camera.tipologia || 'Standard';
            document.getElementById('cameraStato').value = camera.stato;
            document.getElementById('cameraImmagine').value = camera.immagineUrl;
            document.getElementById('cameraDescrizione').value = camera.descrizione;
        } catch (error) {
            console.error('Errore caricamento camera:', error);
            return;
        }
    }
    
    modalCamera.show();
}

async function salvaCamera() {
    const id = document.getElementById('cameraId').value;
    const camera = {
        id: id ? parseInt(id) : 0,
        nome: document.getElementById('cameraNome').value,
        numero: document.getElementById('cameraNumero').value,
        prezzo: parseFloat(document.getElementById('cameraPrezzo').value),
        tipologia: document.getElementById('cameraTipologia').value,
        stato: document.getElementById('cameraStato').value,
        immagineUrl: document.getElementById('cameraImmagine').value,
        descrizione: document.getElementById('cameraDescrizione').value
    };

    const method = id ? 'PUT' : 'POST';
    const url = id ? `/api/camere/${id}` : '/api/camere';

    try {
        const response = await fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(camera)
        });

        if (response.ok) {
            modalCamera.hide();
            caricaDatiAdmin();
        } else {
            alert('Errore nel salvataggio');
        }
    } catch (error) {
        console.error('Errore:', error);
    }
}

async function eliminaCamera(id) {
    if (!confirm('Sei sicuro di voler eliminare questa camera?')) return;

    try {
        const response = await fetch(`/api/camere/${id}`, { method: 'DELETE' });
        if (response.ok) {
            caricaDatiAdmin();
        } else {
            alert('Errore durante l\'eliminazione');
        }
    } catch (error) {
        console.error('Errore:', error);
    }
}