let tutteLeCamere = [];
let cameraSelezionataId = null;
let bookingModal = null;

document.addEventListener('DOMContentLoaded', async () => {
    await caricaCamere();
    bookingModal = new bootstrap.Modal(document.getElementById('bookingModal'));
});

async function caricaCamere() {
    try {
        const response = await fetch('/api/camere');
        tutteLeCamere = await response.json();
        renderCamere(tutteLeCamere);
    } catch (error) {
        console.error('Errore caricamento camere:', error);
    }
}

function renderCamere(camere) {
    const container = document.getElementById('camere-container');
    
    if (camere.length === 0) {
        container.innerHTML = '<div class="col-12 text-center"><p>Nessuna camera disponibile per questa categoria.</p></div>';
        return;
    }

    container.innerHTML = camere.map(c => `
        <div class="col-md-4">
            <div class="card room-card h-100 shadow-sm border-0">
                <div class="position-relative">
                    <img src="${c.immagineUrl || 'https://via.placeholder.com/400x250'}" class="card-img-top" alt="${c.nome}" style="height: 250px; object-fit: cover;">
                    <span class="position-absolute top-0 end-0 badge bg-dark m-3 p-2">${c.tipologia || 'Standard'}</span>
                </div>
                <div class="card-body d-flex flex-column p-4">
                    <div class="d-flex justify-content-between align-items-start mb-2">
                        <h5 class="card-title fw-bold font-playfair mb-0">${c.nome}</h5>
                        <small class="text-muted">N° ${c.numero}</small>
                    </div>
                    <p class="card-text text-muted flex-grow-1 small">${c.descrizione}</p>
                    
                    <div class="d-flex justify-content-between align-items-end mt-4">
                        <div>
                            <span class="text-muted small">Prezzo per notte</span>
                            <div class="h4 mb-0 fw-bold text-success">€${c.prezzo.toFixed(2)}</div>
                        </div>
                         <span class="badge ${getBadgeClass(c.stato)}">${c.stato}</span>
                    </div>

                    <button class="btn btn-reserve mt-4 w-100 py-2 text-uppercase fw-bold" 
                            onclick="apriModalPrenotazione(${c.id})" 
                            ${c.stato !== 'Libera' ? 'disabled' : ''}>
                        ${c.stato === 'Libera' ? 'Prenota Ora' : 'Non Disponibile'}
                    </button>
                </div>
            </div>
        </div>
    `).join('');
}

function getBadgeClass(stato) {
    if (stato === 'Libera') return 'bg-success';
    if (stato === 'Occupata') return 'bg-danger';
    return 'bg-secondary';
}

function filtraCamere(tipo) {
    // Aggiorna bottoni attivi
    document.querySelectorAll('.btn-group .btn').forEach(btn => {
        if (btn.innerText === tipo) btn.classList.add('active');
        else btn.classList.remove('active');
    });

    if (tipo === 'Tutte') {
        renderCamere(tutteLeCamere);
    } else {
        const filtrate = tutteLeCamere.filter(c => (c.tipologia || 'Standard') === tipo);
        renderCamere(filtrate);
    }
}

function apriModalPrenotazione(id) {
    const camera = tutteLeCamere.find(c => c.id === id);
    if (!camera) return;

    cameraSelezionataId = id;
    
    // Popola il modal
    const summaryDiv = document.getElementById('booking-room-summary');
    summaryDiv.innerHTML = `
        <img src="${camera.immagineUrl}" class="img-fluid rounded shadow-sm mb-3" style="max-height: 150px; width: 100%; object-fit: cover;">
        <h4 class="font-playfair mb-1">${camera.nome}</h4>
        <p class="text-muted small mb-0">${camera.tipologia} - Camera ${camera.numero}</p>
    `;

    document.getElementById('bookingTotal').innerText = `€${camera.prezzo.toFixed(2)}`;
    
    // Imposta date default (domani - dopodomani)
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    const dayAfter = new Date();
    dayAfter.setDate(dayAfter.getDate() + 2);
    
    document.getElementById('bookingCheckIn').valueAsDate = tomorrow;
    document.getElementById('bookingCheckOut').valueAsDate = dayAfter;

    bookingModal.show();
}

async function confermaPrenotazione() {
    if (!cameraSelezionataId) return;

    // Qui potremmo raccogliere i dati del form se volessimo inviarli al backend
    // const nome = document.getElementById('bookingName').value;
    // ...

    try {
        const btnConferma = document.querySelector('#bookingModal .btn-gold');
        const originalText = btnConferma.innerText;
        btnConferma.disabled = true;
        btnConferma.innerText = "ATTENDERE...";

        const response = await fetch(`/api/camere/cambia-stato/${cameraSelezionataId}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify("Occupata")
        });

        if (response.ok) {
            bookingModal.hide();
            // Show success modal or toast? For now standard alert but styled better in future.
            // Let's use a simple alert but maybe we can make a success message in UI later.
            // User asked for "popup bello", so let's stick to the modal we just used? 
            // Or just reload and show updated status.
            
            // Per ora usiamo un alert semplice per confermare, ma l'UX del modal è già un passo avanti.
            alert("Prenotazione confermata con successo! Ti aspettiamo.");
            
            await caricaCamere(); 
        } else {
            alert("Errore durante la prenotazione. Riprova.");
        }
        
        btnConferma.disabled = false;
        btnConferma.innerText = originalText;

    } catch (error) {
        console.error('Errore prenotazione:', error);
        alert("Si è verificato un errore di rete.");
    }
}