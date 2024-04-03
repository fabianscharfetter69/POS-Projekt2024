const weekdays = ['So', 'Mo', 'Di', 'Mi', 'Do', 'Fr', 'Sa'];
const monthNames = ['Januar', 'Februar', 'März', 'April', 'Mai', 'Juni', 'Juli', 'August', 'September', 'Oktober', 'November', 'Dezember'];

let currentDate = new Date();

function renderCalendar() {
    const monthYear = document.getElementById('monthYear');
    monthYear.textContent = `${monthNames[currentDate.getMonth()]} ${currentDate.getFullYear()}`;

    const firstDayOfMonth = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
    const daysInMonth = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0).getDate();
    const startingDayOfWeek = firstDayOfMonth.getDay();

    const daysList = document.getElementById('days');
    daysList.innerHTML = '';

    for (let i = 0; i < startingDayOfWeek; i++) {
        const prevDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), -i);
        const listItem = document.createElement('li');
        listItem.textContent = prevDate.getDate();
        listItem.classList.add('past');
        daysList.appendChild(listItem);
    }

    for (let i = 1; i <= daysInMonth; i++) {
        const listItem = document.createElement('li');
        listItem.textContent = i;
        if (currentDate.getFullYear() === new Date().getFullYear() && currentDate.getMonth() === new Date().getMonth() && i === new Date().getDate()) {
            listItem.classList.add('today');
        } else if (new Date(currentDate.getFullYear(), currentDate.getMonth(), i) < new Date()) {
            listItem.classList.add('past');
        }
        listItem.addEventListener('click', () => openModal(listItem));
        daysList.appendChild(listItem);
    }
}

function prevMonth() {
    currentDate.setMonth(currentDate.getMonth() - 1);
    renderCalendar();
}

function nextMonth() {
    currentDate.setMonth(currentDate.getMonth() + 1);
    renderCalendar();
}

// Öffne die Modalbox zum Hinzufügen von Geburtstagen
function openModal(dayElement) {
    const day = dayElement.textContent;
    const monthYear = document.getElementById('monthYear').textContent;
    const [month, year] = monthYear.split(' ');
    document.getElementById('birthdayName').value = '';
    document.getElementById('birthdayDay').value = day;
    document.getElementById('birthdayMonth').value = month;
    document.getElementById('birthdayYear').value = year;
    modal.style.display = "block";
}

// Schließe die Modalbox
function closeModal() {
    modal.style.display = "none";
}

// Speichere den Geburtstag und färbe den entsprechenden Tag ein
function saveBirthday() {
    const name = document.getElementById('birthdayName').value;
    const day = document.getElementById('birthdayDay').value;
    const month = document.getElementById('birthdayMonth').value;
    const year = document.getElementById('birthdayYear').value;
    const dayElement = document.querySelector(`.days li:nth-child(${day})`);
    dayElement.style.backgroundColor = "#FFD700"; // Beispiel für die Farbe Gelb
    closeModal();
}

// Rufe renderCalendar auf, um den Kalender zu initialisieren
renderCalendar();
