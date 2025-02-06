function toggleDropdown() {
    document.getElementById("dropdown-profile-menu").classList.toggle("show");
}

function toggleSidebar() {
    document.body.classList.toggle('sidebar-collapsed');
}

// Close the dropdown if the user clicks outside of it
window.onclick = function (event) {
    if (!event.target.matches('.profile-icon')) {
        var dropdowns = document.getElementsByClassName("dropdown-profile-content");
        for (var i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}

document.getElementById('btn-main-login').addEventListener('click', function () {
    if (isAuthenticated === 'true') {
        var userRole = getUserRole(); // Function to get the user's role
        if (userRole === 'Admin') {
            window.location.href = '/Admin/SecurePage';
        } else if (userRole === 'Donor') {
            window.location.href = '/Donor/SecurePage';
        } else if (userRole === 'Hospital') {
            window.location.href = '/Hospital/SecurePage';
        }
    }
});

function getUserRole() {
    // This function should return the user's role based on your application's logic
    // For example, you can store the role in a cookie or local storage and retrieve it here
    return localStorage.getItem('userRole');
}




