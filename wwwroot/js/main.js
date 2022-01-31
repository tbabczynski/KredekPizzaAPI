function display_menu(myArr) {
    // alert("tu");
    var table = document.createElement("table");
    var thead = document.createElement("thead");
    var tbody = document.createElement("tbody");

    table.appendChild(thead);
    table.appendChild(tbody);

    var th = thead.insertRow();
    th.insertCell();
    th.insertCell();
    th.insertCell();

    var tableData = myArr.map(function (value) {
        return (
            `<tr>
                            <td>${value.id}</td>
                            <td>${value.name}</td>
                            <td>${value.cost}</td>
                            <td>${value.description}</td>
                            <td class="text-center"><a class="btn btn-primary" href="route.html?id=${value.id}" role="button">Details</a></td>
                        </tr>`
        );
    }).join("");

    tbody.innerHTML = tableData;

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
