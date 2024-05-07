import Accounts from "./components/Accounts";
import CalendarEvents from "./components/CalendarEvents";
import Calendars from "./components/Calendars";
import Categories from "./components/Categories";
import Home from "./components/Home";
import ImportData from "./components/ImportData";
import Roles from "./components/Roles";
import Transactions from "./components/Transactions";

const AppRoutes = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/accounts',
        element: <Accounts />
    },
    {
        path: '/roles',
        element: <Roles />
    },
    {
        path: '/calendars',
        element: <Calendars />
    },
    {
        path: '/calendar',
        element: <CalendarEvents />
    },
    {
        path: '/categories',
        element: <Categories />
    },
    {
        path: '/transactions',
        element: <Transactions />
    },
    {
        path: "/import",
        element: <ImportData />
    }
];

export default AppRoutes;
