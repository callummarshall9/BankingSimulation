import Accounts from "./components/Accounts";
import EditCalendar from "./components/EditCalendar";
import Calendars from "./components/Calendars";
import Categories from "./components/Categories";
import ImportData from "./components/ImportData";
import Roles from "./components/Roles";
import Transactions from "./components/Transactions";

const AppRoutes = [
    {
        index: true,
        element: <Accounts />
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
        element: <EditCalendar />
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
