
function SetAsActiveSection(SectionName, reloadFlag = true)
{
    document.getElementById(SectionName).hidden = false;

    if(reloadFlag)
    {
        localStorage.setItem("CurrentDashboard", SectionName)
        console.log(localStorage.getItem("CurrentDashboard"));
    }

    const SectionsNames = [ 
        "Projects-section",
        "Dashboard-section", 
        "Settings-section",
        "Clients-section"
    ]

    SectionsNames.forEach((value) => 
    {
        if(value.toLocaleUpperCase() !== SectionName.toLocaleUpperCase())
        {
            const Element  = document.getElementById(value);
            if(Element != null){
                Element.hidden = true;
            }
        }

    })

}

const mainContent = document.getElementById("mainContent");

const Settings = document.getElementById("Dev-Settings");
const Projects = document.getElementById("Dev-Projects");
const Clients = document.getElementById("Dev-Clients");
const Dashboard = document.getElementById("Dev-Dashboard");


function SetAsActiveSideNav(navElement)
{
    navElement.classList.add("text-blue-600", "bg-blue-50", "border-r-4", "border-blue-600", "hover:bg-blue-50");

    localStorage.setItem("current-nav", navElement.id);

    const elements = [Settings, Projects, Clients, Dashboard];
    elements.forEach((value) => {
        if(value !== navElement)
        {
            value.classList.remove("text-blue-600", "bg-blue-50", "border-r-4", "border-blue-600", "hover:bg-blue-50"); 
            value.classList.add("text-gray-600", "hover:bg-gray-50", "hover:text-blue-600");
        }
    })
}


Settings.addEventListener("click", () => {
    SetAsActiveSideNav(Settings);
    SetAsActiveSection("Settings-section");
})

Projects.addEventListener("click", () => {
    SetAsActiveSideNav(Projects);
    SetAsActiveSection("Projects-section");
})

Clients.addEventListener("click", () => {
    SetAsActiveSideNav(Clients);
    SetAsActiveSection("Clients-section");
})


Dashboard.addEventListener("click", () => {
    SetAsActiveSideNav(Dashboard);
    SetAsActiveSection("Dashboard-section");
})

{
    const mainContent = document.getElementById('mainContent');
    const sidebar = document.getElementById('sidebar');
    const toggleBtn = document.getElementById('toggleBtn');
    const toggleIcon = document.getElementById('toggleIcon');
    const sidebarTitle = document.getElementById('sidebarTitle');
    const navTexts = [1, 2, 3, 4].map(i => document.getElementById(`navText${i}`));

    let isCollapsed = false;



    toggleBtn.addEventListener('click', () => {
        isCollapsed = !isCollapsed;

        if (isCollapsed) {
            sidebar.classList.remove('w-64');
            sidebar.classList.add('w-20');
            mainContent.classList.remove('ml-64');
            mainContent.classList.add('ml-20');
            toggleIcon.classList.remove('fa-chevron-left');
            toggleIcon.classList.add('fa-chevron-right');
            sidebarTitle.classList.add('opacity-0', 'invisible');
            navTexts.forEach(text => text.classList.add('opacity-0', 'invisible'));

            toggleBtn.classList.add('slideButtonLeft');
        } else {
            sidebar.classList.remove('w-20');
            sidebar.classList.add('w-64');
            mainContent.classList.remove('ml-20');
            mainContent.classList.add('ml-64');
            toggleIcon.classList.remove('fa-chevron-right');
            toggleIcon.classList.add('fa-chevron-left');
            sidebarTitle.classList.remove('opacity-0', 'invisible');
            navTexts.forEach(text => text.classList.remove('opacity-0', 'invisible'));

            toggleBtn.classList.remove('slideButtonLeft');
        }
    });

    // Mobile responsiveness
    if (window.innerWidth < 768) {
        sidebar.classList.add('-translate-x-full');
        mainContent.classList.remove('ml-64');
        mainContent.classList.add('ml-0');
    }
}
