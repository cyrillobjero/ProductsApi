let token = null;
let username = null;

function register() {

    const u =
        document.getElementById("regUser").value;

    const p =
        document.getElementById("regPass").value;

    fetch(
        "/api/Auth/register",
        {
            method: "POST",

            headers: {
                "Content-Type": "application/json"
            },

            body: JSON.stringify({
                username: u,
                password: p
            })
        })
        .then(response => {

            document.getElementById(
                "output").innerText =
                "Register Status: "
                + response.status;
        });
}

function login() {

    const u =
        document.getElementById("loginUser").value;

    const p =
        document.getElementById("loginPass").value;

    fetch(
        "/api/Auth/login",
        {
            method: "POST",

            headers: {
                "Content-Type": "application/json"
            },

            body: JSON.stringify({
                username: u,
                password: p
            })
        })
        .then(response => {

            document.getElementById(
                "output").innerText =
                "Login Status: "
                + response.status;

            if (!response.ok) {
                return;
            }

            return response.json();
        })
        .then(data => {

            if (!data)
                return;

            token = data.token;

            username = u;

            localStorage.setItem(
                "token",
                token);

            document.getElementById(
                "username").innerText =
                "Logged in as " +
                username;
        });
}

function authFetch(url, options) {

    options = options || {};

    options.headers =
        options.headers || {};

    options.headers["Authorization"] =
        "Bearer " + token;

    return fetch(url, options);
}

function logout() {

    token = null;

    username = null;

    localStorage.removeItem(
        "token");

    document.getElementById(
        "username").innerText = "";

    document.getElementById(
        "output").innerText =
        "Logged out.";
}

function addProduct() {

    const product = {
        name:
            document.getElementById("productName").value,

        price:
            parseFloat(
                document.getElementById("productPrice").value),

        sku:
            document.getElementById("productSku").value
    };

    authFetch(
        "/api/products",
        {
            method: "POST",

            headers: {
                "Content-Type":
                    "application/json"
            },

            body:
                JSON.stringify(product)
        })
        .then(response => {

            document.getElementById(
                "output").innerText =
                "Add Product Status: "
                + response.status;
        });
}

function getProducts() {

    fetch("/api/products")
        .then(response =>
            response.json())
        .then(data => {

            document.getElementById(
                "output").innerText =
                JSON.stringify(
                    data,
                    null,
                    2);
        });
}