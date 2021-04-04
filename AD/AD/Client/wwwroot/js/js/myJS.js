// Navbar
function NavbarToggler() {
    document.getElementById("sidenavWrapper").classList.add("nav-active");
    document.getElementById("headerArea").classList.add("header-out");
    document.getElementById("footerNav").classList.add("footer-out");
    document.getElementsByClassName("sidenav-black-overlay")[0].classList.add("active");
}

function goHomeBtn() {
    document.getElementById("sidenavWrapper").classList.remove("nav-active");
    document.getElementById("headerArea").classList.remove("header-out");
    document.getElementById("footerNav").classList.remove("footer-out");
    document.getElementsByClassName("sidenav-black-overlay")[0].classList.remove("active");
}

function SidenavBlackOverlay() {
    document.getElementById("sidenavWrapper").classList.remove("nav-active");
    document.getElementById("headerArea").classList.remove("header-out");
    document.getElementById("footerNav").classList.remove("footer-out");
    document.getElementsByClassName("sidenav-black-overlay")[0].classList.remove("active");
}

//GetDarkMode Function
function GetDarkMode() {
    return localStorage.getItem('theme');
}

// Dark Mode JS
function darkMode() {

    var toggleSwitch = document.getElementById('darkSwitch');
    var currentTheme = localStorage.getItem('theme');

    if (currentTheme) {
        document.documentElement.setAttribute('data-theme', currentTheme);
        if (currentTheme === 'dark') {
            if (toggleSwitch) {
                toggleSwitch.checked = false;
                console.log("Hello World0");
            }
        }
          
        console.log("Hello World1");
    }

    function switchTheme(e) {
        if (e.target.checked) {
            document.documentElement.setAttribute('data-theme', 'dark');
            localStorage.setItem('theme', 'dark');
            console.log("Hello World2");

        } else {
            document.documentElement.setAttribute('data-theme', 'light');
            localStorage.setItem('theme', 'light');
            console.log("Hello World3");

        }
    }


    if (toggleSwitch) {
        toggleSwitch.addEventListener('change', switchTheme, false);
        console.log("Hello World4");
    }
}
function GetSubByCat() {
    console.log("Hello World5");

    //document.getElementById("sidenavWrapper").classList.add("nav-active");
    //document.getElementById("headerArea").classList.add("header-out");
    //document.getElementById("footerNav").classList.add("footer-out");
    //document.getElementsByClassName("sidenav-black-overlay")[0].classList.add("active");
}

