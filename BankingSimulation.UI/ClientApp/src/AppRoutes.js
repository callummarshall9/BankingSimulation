import { Accounts } from "./components/Accounts";
import { Home } from "./components/Home";
import { ImportData } from "./components/ImportData";

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
        path: "/import",
        element: <ImportData />
    }
];

export default AppRoutes;
