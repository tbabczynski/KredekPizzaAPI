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
                            <td class="cook-only"><button class="btn btn-danger" onclick="open_delete_modal(${value.id})">Eliminare</button></td>
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
    // Zapamiętaj stan "zalogowania" w sessionStorage
    sessionStorage.setItem('isCook', 'true');

    // Dodaj klasę do body
    document.body.classList.add('logged-as-cook');
    var logInOutButton = document.getElementById('log-in-out');
    logInOutButton.textContent = 'Siesta';
    logInOutButton.onclick = logout;

    //alert('Logged in as Cook');
}

// Przy ładowaniu strony sprawdź czy użytkownik jest "zalogowany"
window.onload = function () {
    if (sessionStorage.getItem('isCook') === 'true') {
        document.body.classList.add('logged-as-cook');
        var logInOutButton = document.getElementById('log-in-out');
        logInOutButton.textContent = 'Siesta';
        logInOutButton.onclick = logout;
    } else {
        var logInOutButton = document.getElementById('log-in-out');
        logInOutButton.textContent = 'Pizzaiolo';
        logInOutButton.onclick = login_as_cook;
    }
    // Obsługa formularza dodawania pizzy
    document.getElementById('add-pizza-form').addEventListener('submit', function (e) {
        e.preventDefault();
        submit_add_pizza();
    });
}

// funkcja wylogowania
function logout() {
    sessionStorage.removeItem('isCook');
    document.body.classList.remove('logged-as-cook');

    var logInOutButton = document.getElementById('log-in-out');
    logInOutButton.textContent = 'Pizzaiolo';
    logInOutButton.onclick = login_as_cook;
}

function reset_menu() {
    // Wywołaj endpoint który czyści sesję
    fetch('/api/pizza/reset', { method: 'POST' })
        .then(() => {
            document.getElementById("Menu_div").innerHTML = "";
            logout();
            //show_menu("/api/pizza/menu");
            //alert("Menu zresetowane!");
        });
}

// === MODAL DO DODAWANIA PIZZY ===

function open_add_pizza_modal() {
    document.getElementById('add-pizza-modal').style.display = 'block';
    document.getElementById('pizza-name').focus();
}

function close_add_pizza_modal() {
    document.getElementById('add-pizza-modal').style.display = 'none';
    document.getElementById('add-pizza-form').reset();
}

function get_api_error_message(xhr) {
    if (!xhr || !xhr.responseText) {
        return xhr && xhr.statusText ? xhr.statusText : 'Nieznany błąd.';
    }

    try {
        var payload = JSON.parse(xhr.responseText);
        var details = [];

        if (payload.title) {
            details.push(payload.title);
        }

        if (payload.errors) {
            Object.keys(payload.errors).forEach(function (field) {
                payload.errors[field].forEach(function (message) {
                    details.push(field + ': ' + message);
                });
            });
        }

        if (details.length > 0) {
            return details.join('\n');
        }
    } catch (e) {
    }

    return xhr.statusText || 'Nieznany błąd.';
}

function submit_add_pizza() {
    var name = document.getElementById('pizza-name').value;
    var cost = parseFloat(document.getElementById('pizza-cost').value);
    var description = document.getElementById('pizza-description').value;

    var pizzaData = {
        name: name,
        cost: cost,
        description: description
    };

    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/api/pizza", true);
    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 200 || this.status === 201) {
                close_add_pizza_modal();
                document.getElementById("Menu_div").innerHTML = "";
                show_menu("/api/pizza/menu");
            } else {
                alert("Błąd podczas dodawania pizzy:\n" + get_api_error_message(this));
            }
        }
    };
    xhr.send(JSON.stringify(pizzaData));
}

// === MODAL DO USUWANIA PIZZY ===

var pizzaToDelete = null;

function open_delete_modal(pizza_id) {
    pizzaToDelete = pizza_id;
    document.getElementById('delete-confirm-modal').style.display = 'block';
}

function close_delete_modal() {
    document.getElementById('delete-confirm-modal').style.display = 'none';
    pizzaToDelete = null;
}

function confirm_delete() {
    if (pizzaToDelete === null) return;

    var xhr = new XMLHttpRequest();
    xhr.open("DELETE", `/api/pizza/${pizzaToDelete}`, true);
    xhr.onreadystatechange = function () {
        if (this.readyState === 4) {
            if (this.status === 204) {
                close_delete_modal();
                document.getElementById("Menu_div").innerHTML = "";
                show_menu("/api/pizza/menu");
            } else {
                alert("Błąd podczas usuwania pizzy: " + this.statusText);
            }
        }
    };
    xhr.send();
}

// Zamknij modal po kliknięciu poza nim
window.onclick = function (event) {
    var addModal = document.getElementById('add-pizza-modal');
    var deleteModal = document.getElementById('delete-confirm-modal');
    if (event.target == addModal) {
        close_add_pizza_modal();
    }
    if (event.target == deleteModal) {
        close_delete_modal();
    }
}

// === KONIEC MODALI ===

