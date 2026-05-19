const STORAGE_KEY = "app_state_initialized";

function getState() {
    return localStorage.getItem(STORAGE_KEY) === "true";
}

function setState(value) {
    localStorage.setItem(STORAGE_KEY, value.toString());
}

if (!getState()) {
    // First time app is opened
    setState(true);
}

const ReloadToLastViewedPage = () => {
    const LastViewdSection = localStorage.getItem("CurrentDashboard");
    const LastActiveNavElement = localStorage.getItem("current-nav");

    if(LastViewdSection != null)
    {
        SetAsActiveSection(LastViewdSection);
        const Element  = document.getElementById(LastActiveNavElement);
        SetAsActiveSideNav(Element);
    }

}



if(getState())
{
    //ReloadToLastViewedPage();
}

window.addEventListener("load", () => {
        setTimeout(() => {
            document.getElementById("page-loader").classList.add("hidden");
            document.getElementById("page-content").classList.remove("hidden");
        }, 400); 
    });


