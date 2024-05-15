document.addEventListener("DOMContentLoaded", function() {
    const currentDateElement = document.querySelector('.current-date');
    const daysListElement = document.querySelector('.days');
    const prevButton = document.getElementById('prev');
    const nextButton = document.getElementById('next');
    const birthdayListElement = document.querySelector('.birthday-list-content');
    const allButton = document.getElementById('allButton');
    const monthButton = document.getElementById('monthButton');

    // Aktuelles Datum erhalten
    const currentDate = new Date();
    let currentYear = currentDate.getFullYear();
    let currentMonth = currentDate.getMonth();
    let currentDay = currentDate.getDate();

    // Funktion zum Erstellen des Kalenders
    function createCalendar(year, month) {
        const daysOfWeek = ['So', 'Mo', 'Di', 'Mi', 'Do', 'Fr', 'Sa'];
        const daysInPrevMonth = new Date(year, month, 0).getDate();
        const daysInMonth = new Date(year, month + 1, 0).getDate();
        const firstDayOfMonth = new Date(year, month, 1).getDay();

        // Leeren des aktuellen Kalenders
        daysListElement.innerHTML = '';

        // Tage des vorherigen Monats hinzufügen
        for (let i = 0; i < firstDayOfMonth; i++) {
            const listItem = document.createElement('li');
            listItem.textContent = daysInPrevMonth - firstDayOfMonth + i + 1;
            listItem.classList.add('day');
            listItem.classList.add('inactive');
            daysListElement.appendChild(listItem);
        }

        // Erstellen der Tage des aktuellen Monats
        for (let i = 0; i < daysInMonth; i++) {
            const dayOfWeek = (firstDayOfMonth + i) % 7; // Um den richtigen Wochentag zu erhalten
            const listItem = document.createElement('li');
            listItem.textContent = i + 1;
            listItem.classList.add('day');
            if (dayOfWeek === 0) {
                listItem.classList.add('sunday');
            }
            if (i + 1 === currentDay && year === currentDate.getFullYear() && month === currentDate.getMonth()) {
                listItem.classList.add('active');
            }

            daysListElement.appendChild(listItem);
        }

        // Tage des nächsten Monats hinzufügen, um die Woche abzuschließen
        const remainingDays = 42 - (daysInMonth + firstDayOfMonth); // Maximal 42 Tage im Kalender
        for (let i = 0; i < remainingDays; i++) {
            const listItem = document.createElement('li');
            listItem.textContent = i + 1;
            listItem.classList.add('day');
            listItem.classList.add('inactive');
            daysListElement.appendChild(listItem);
        }

        // Anzeigen des aktuellen Monats und Jahres
        currentDateElement.textContent = `${month + 1}/${year}`;
    }

    // Funktion zum Anzeigen der Geburtstage
    function displayBirthdays(birthdays) {
        birthdays.sort((a, b) => {
            if (a.month !== b.month) {
                return a.month - b.month;
            }
            return a.day - b.day;
        });

        birthdayListElement.innerHTML = '';
        birthdays.forEach(birthday => {
            const listItem = document.createElement('div');
            listItem.classList.add('birthday-item');
            listItem.textContent = `${birthday.name} - ${birthday.day}.${birthday.month}.${birthday.year}`;
            birthdayListElement.appendChild(listItem);
        });

    }

    function displayMonthBithdays(){
        const currentMonthBirthdays = [];

        fetch('http://localhost:8081/geb-liste/geburtstage')
            .then(response => response.json())
            .then(data => {
                data.forEach(birthday => {              // Geburtstage des aktuellen Monats filtern
                    if (parseInt(birthday.month) - 1 === currentMonth) {
                        currentMonthBirthdays.push(birthday);
                    }
                });

                // Geburtstage des aktuellen Monats anzeigen
                displayBirthdays(currentMonthBirthdays);
                document.querySelector('.birthday-list-title').textContent = 'Geburtstage in dem Monat';
            })
            .catch(error => {
                console.error('Error fetching birthdays:', error);
            });
    }

    // Kalender beim Laden der Seite erstellen
    createCalendar(currentYear, currentMonth);

    // Geburtstage von der API laden und alle anzeigen
    fetch('http://localhost:8081/geb-liste/geburtstage')
        .then(response => response.json())
        .then(data => {
            displayBirthdays(data);             // Geburtstage anzeigen
        })
        .catch(error => {
            console.error('Error fetching birthdays:', error);
        });

    // Event Listener für vorherigen Monat
    prevButton.addEventListener('click', function() {
        currentMonth--;
        if (currentMonth < 0) {
            currentMonth = 11;
            currentYear--;
        }
        createCalendar(currentYear, currentMonth);
        displayMonthBithdays();
    });

    // Event Listener für nächsten Monat
    nextButton.addEventListener('click', function() {
        currentMonth++;
        if (currentMonth > 11) {
            currentMonth = 0;
            currentYear++;
        }
        createCalendar(currentYear, currentMonth);
        displayMonthBithdays();
    });

    // Event Listener für den "Alle Geburtstage anzeigen" Button
    allButton.addEventListener('click', function() {
        fetch('http://localhost:8081/geb-liste/geburtstage')
            .then(response => response.json())
            .then(data => {
                // Geburtstage anzeigen
                displayBirthdays(data);
                document.querySelector('.birthday-list-title').textContent = 'Alle Geburtstage';
            })
            .catch(error => {
                console.error('Error fetching birthdays:', error);
            });
    });

    // Event Listener für den "Geburtstage des aktuellen Monats anzeigen" Button
    monthButton.addEventListener('click', function() {
        displayMonthBithdays();
    });
});