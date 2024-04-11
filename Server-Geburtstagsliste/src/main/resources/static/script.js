const daysTag = document.querySelector(".days"),
    currentDate = document.querySelector(".current-date"),
    prevNextIcon = document.querySelectorAll(".icons span");

// getting new date, current year and month
let date = new Date(),
    currYear = date.getFullYear(),
    currMonth = date.getMonth();

// storing full name of all months in array
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
        // adding active class to li if the current day, month, and year matched
        let isToday = i === date.getDate() && currMonth === new Date().getMonth()
        && currYear === new Date().getFullYear() ? "active" : "";
        liTag += `<li class="${isToday}">${i}</li>`;
    }

    for (let i = lastDayofMonth; i < 6; i++) { // creating li of next month first days
        liTag += `<li class="inactive">${i - lastDayofMonth + 1}</li>`
    }
    currentDate.innerText = `${months[currMonth]} ${currYear}`; // passing current mon and yr as currentDate text
    daysTag.innerHTML = liTag;
}
renderCalendar();

prevNextIcon.forEach(icon => { // getting prev and next icons
    icon.addEventListener("click", () => { // adding click event on both icons
        // if clicked icon is previous icon then decrement current month by 1 else increment it by 1
        currMonth = icon.id === "prev" ? currMonth - 1 : currMonth + 1;

        if(currMonth < 0 || currMonth > 11) { // if current month is less than 0 or greater than 11
            // creating a new date of current year & month and pass it as date value
            date = new Date(currYear, currMonth, new Date().getDate());
            currYear = date.getFullYear(); // updating current year with new date year
            currMonth = date.getMonth(); // updating current month with new date month
        } else {
            date = new Date(); // pass the current date as date value
        }
        renderCalendar(); // calling renderCalendar function
    });
});

const dayOnClick = (event) => {
    const clickedDay = event.target;
    const popup = document.getElementById("popup");

    // Open the popup
    popup.style.display = "block";

    const birthdayForm = document.getElementById("birthdayForm");
    birthdayForm.addEventListener("submit", (e) => {
        e.preventDefault(); // prevent form submission

        const dateInput = document.getElementById("dateInput").value;
        const nameInput = document.getElementById("nameInput").value;
        const errorMsg = document.getElementById("errorMsg");

        if (isValidDate(dateInput)) {
            clickedDay.classList.add("birthday");
            clickedDay.innerHTML = `${nameInput}<br>${clickedDay.innerHTML}`;
            // Close the popup
            popup.style.display = "none";
        } else {
            errorMsg.innerText = "Bitte geben Sie ein gültiges Datum ein.";
        }
    });

    // Close the popup when clicking the close button
    const closeBtn = document.querySelector(".close");
    closeBtn.addEventListener("click", () => {
        popup.style.display = "none";
        resetForm(); // Reset form inputs and error message
    });

    // Close the popup when clicking outside of it
    window.addEventListener("click", (event) => {
        if (event.target == popup) {
            popup.style.display = "none";
            resetForm(); // Reset form inputs and error message
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

