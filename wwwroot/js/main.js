function insertHCell(trow) {
    var th = document.createElement("th");
    trow.appendChild(th);
    return th;
}

function display_menu(myArr) {
    // alert("tu");
    var table = document.createElement("table");
    var thead = table.createTHead();
    var tbody = table.createTBody();

    var hrow = thead.insertRow();

    insertHCell(hrow).innerText = "Id";
    insertHCell(hrow).innerText = "Name";
    insertHCell(hrow).innerText = "Cost";
    insertHCell(hrow).innerText = "Description";
    var act_cell = insertHCell(hrow)
    act_cell.innerText = "Actions";
    act_cell.className = "cook-only";

    var tableData = myArr.map(function (value) {
        return (
            `<tr>
                            <td class="text-center"><a class="btn" target="_blank" href="/order-details.html?id=${value.id}" role="button">${value.id}</a></td>
                            <td><a class="btn" target="_blank" href="/order-details.html?id=${value.name}"
                                role="button">${value.name}</a></td>
                            <td>${value.cost}</td>
                            <td>${value.description}</td>
                            <td class="cook-only"><button class="btn btn-danger" onclick="delete_pizza(${value.id})">Eliminare</button></td>
                        </tr>`
        );
    }).join("");

    tbody.innerHTML = tableData;

    //document.getElementById("Menu_div").replaceChildren(table);
    document.getElementById("Menu_div").appendChild(table);
}

function show_menu(hrf) {
    var html = new XMLHttpRequest();

    html.onreadystatechange = function () {
        // alert(this.readyState);
        if (this.readyState === 4 && this.status === 200) {
            //alert(this.responseText)
            var myArr = JSON.parse(this.responseText);
            display_menu(myArr);
        }
    };

    html.onerror = function () {
        alert("Error " + this.statusText + ".");
    };
    html.open("GET", hrf);
    // html.setRequestHeader("X-Requested-With","XMLHttpRequest")
    // html.setRequestHeader('Access-Control-Allow-Origin', '*')
    html.setRequestHeader("Content-type", "application/json");
    html.send();
}

function login_as_cook() {
    // Zapamiętaj stan "zalogowania" w localStorage
    localStorage.setItem('isCook', 'true');

    // Dodaj klasę do body
    document.body.classList.add('logged-as-cook');
    var logInOutButton = document.getElementById('log-in-out');
    logInOutButton.textContent = 'Siesta';
    logInOutButton.onclick = logout;

    //alert('Logged in as Cook');
}

// Przy ładowaniu strony sprawdź czy użytkownik jest "zalogowany"
window.onload = function () {
    if (localStorage.getItem('isCook') === 'true') {
        document.body.classList.add('logged-as-cook');
        var logInOutButton = document.getElementById('log-in-out');
        logInOutButton.textContent = 'Siesta';
        logInOutButton.onclick = logout;
    } else {
        var logInOutButton = document.getElementById('log-in-out');
        logInOutButton.textContent = 'Pizzaiolo';
        logInOutButton.onclick = login_as_cook;
    }
}

// Opcjonalnie: funkcja wylogowania
function logout() {
    localStorage.removeItem('isCook');
    document.body.classList.remove('logged-as-cook');

    var logInOutButton = document.getElementById('log-in-out');
    logInOutButton.textContent = 'Pizzaiolo';
    logInOutButton.onclick = login_as_cook;
}

function delete_pizza(pizza_id) {
    var confirmation = confirm("Chcesz wyrzucić taką świetną pizzę?");
    if (!confirmation) {
        return; // Użytkownik anulował akcję
    }
    var xhr = new XMLHttpRequest();
    xhr.open("DELETE", `/api/pizza/${pizza_id}`, true);
    xhr.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 204) {
                alert("Pizza usunięta pomyślnie.");
                document.getElementById("Menu_div").innerHTML = ""; // Wyczyść istniejącą zawartość
                show_menu("/api/pizza/menu");
            } else {
                alert("Błąd podczas usuwania pizzy: " + this.statusText);
            }
        }
    };
    xhr.send();
}

function add_pizza() {
    var name = prompt("Podaj nazwę pizzy:");
    if (!name) {
        alert("Nazwa pizzy jest wymagana.");
        return;
    }
    var cost = prompt("Podaj koszt pizzy:");
    if (!cost || isNaN(cost)) {
        alert("Koszt pizzy musi być liczbą.");
        return;
    }
    var description = prompt("Podaj opis pizzy:");
    if (!description) {
        alert("Opis pizzy jest wymagany.");
        return;
    }
    var pizzaData = {
        name: name,
        cost: parseFloat(cost),
        description: description
    };
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/api/pizza", true);
    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 201) {
                alert("Pizza dodana pomyślnie.");
                document.getElementById("Menu_div").innerHTML = ""; // Wyczyść istniejącą zawartość
                show_menu("/api/pizza/menu");
            } else {
                alert("Błąd podczas dodawania pizzy: " + this.statusText);
            }
        }
    };
    xhr.send(JSON.stringify(pizzaData));
}

