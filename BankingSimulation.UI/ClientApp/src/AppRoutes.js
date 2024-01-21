import { Accounts } from "./components/Accounts";
import { Home } from "./components/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/accounts',
    element: <Accounts />
  }
];

export default AppRoutes;
