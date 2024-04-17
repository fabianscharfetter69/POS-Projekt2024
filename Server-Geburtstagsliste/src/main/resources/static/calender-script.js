const daysTag = document.querySelector(".days"),
    currentDate = document.querySelector(".current-date"),
    prevNextIcon = document.querySelectorAll(".icons span");

let date = new Date(),
    currYear = date.getFullYear(),
    currMonth = date.getMonth();

const months = ["Januar", "Februar", "März", "April", "Mai", "Juni", "Juli",
    "August", "September", "Oktober", "November", "Dezember"];

const renderCalendar = () => {
    let firstDayofMonth = new Date(currYear, currMonth, 1).getDay(), // getting first day of month
        lastDateofMonth = new Date(currYear, currMonth + 1, 0).getDate(), // getting last date of month
        lastDayofMonth = new Date(currYear, currMonth, lastDateofMonth).getDay(), // getting last day of month
        lastDateofLastMonth = new Date(currYear, currMonth, 0).getDate(); // getting last date of previous month
    let liTag = "";

    for (let i = firstDayofMonth; i > 0; i--) { // creating li of previous month last days
        liTag += `<li class="inactive">${lastDateofLastMonth - i + 1}</li>`;
    }

    for (let i = 1; i <= lastDateofMonth; i++) { // creating li of all days of current month
        let isToday = i === date.getDate() && currMonth === new Date().getMonth()
        && currYear === new Date().getFullYear() ? "active" : "";
        liTag += `<li class="${isToday}">${i}</li>`;
    }

    for (let i = lastDayofMonth; i < 6; i++) { // creating li of next month first days
        liTag += `<li class="inactive">${i - lastDayofMonth + 1}</li>`
    }
    currentDate.innerText = `${months[currMonth]} ${currYear}`; // passing current mon and yr as currentDate text
    daysTag.innerHTML = liTag;

    // Laden der Geburtstage beim Rendern des Kalenders
    loadBirthdays();
}
renderCalendar();

prevNextIcon.forEach(icon => { // getting prev and next icons
    icon.addEventListener("click", () => { // adding click event on both icons
        currMonth = icon.id === "prev" ? currMonth - 1 : currMonth + 1;

        if(currMonth < 0 || currMonth > 11) {
            date = new Date(currYear, currMonth, new Date().getDate());
            currYear = date.getFullYear();
            currMonth = date.getMonth();
        } else {
            date = new Date(); // pass the current date as date value
        }
        renderCalendar(); // calling renderCalendar function
    });
});





const dayOnClick = (event) => {
    const clickedDay = event.target;
    const popup = document.getElementById("popup");

    popup.style.display = "block";

    const birthdayForm = document.getElementById("birthdayForm");
    birthdayForm.addEventListener("submit", (e) => {
        e.preventDefault();

        const dateInput = document.getElementById("dateInput").value;
        document.getElementById("dateInput").value = null;
        const nameInput = document.getElementById("nameInput").value;
        document.getElementById("nameInput").value = null;
        const errorMsg = document.getElementById("errorMsg");

        if (isValidDate(dateInput)) {
            clickedDay.classList.add("birthday");
            clickedDay.innerHTML = `${nameInput}<br>${clickedDay.innerHTML}`;

            popup.style.display = "none";
        } else {
            errorMsg.innerText = "Bitte geben Sie ein gültiges Datum ein.";
        }
    });

    // Close the popup when clicking the close button
    const closeBtn = document.querySelector(".close");
    closeBtn.addEventListener("click", () => {
        popup.style.display = "none";
        resetForm();
    });

    // Close the popup when clicking outside of it
    window.addEventListener("click", (event) => {
        if (event.target == popup) {
            popup.style.display = "none";
            resetForm();
        }
    });
};

// Überprüfung der Gültigkeit des Datums
const isValidDate = (date) => {
    return !isNaN(Date.parse(date));
};

// Zurücksetzen des Formulars und der Fehlermeldung
const resetForm = () => {
    document.getElementById("birthdayForm").reset();
    document.getElementById("errorMsg").innerText = "";
};






const url = 'http://localhost:8081/geb-liste/geburtstage';

async function fetchData(url) {
    try {
        const response = await fetch(url);
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Fehler beim Abrufen der Daten:', error);
        return [];
    }
}

// Eine Funktion, um die erhaltenen Daten in eine Liste einzufügen
async function createList() {
    const data = await fetchData(url);
    const list = document.createElement('ul');

    // Schleife durch die Daten und Erstellen von Listenelementen für jeden Eintrag
    data.forEach(item => {
        const listItem = document.createElement('li');
        listItem.textContent = `${item.name} - Geburtstag am ${item.day}.${item.month}.${item.year}`;
        list.appendChild(listItem);
    });

    // Die Liste dem DOM hinzufügen
    document.body.appendChild(list);
}
