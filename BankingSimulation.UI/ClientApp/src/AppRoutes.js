import Accounts from "./components/Accounts";
import Categories from "./components/Categories";
import Home from "./components/Home";
import ImportData from "./components/ImportData";
import Roles from "./components/Roles";

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
        path: '/categories',
        element: <Categories />
    },
    {
        path: "/import",
        element: <ImportData />
    }
];

export default AppRoutes;
