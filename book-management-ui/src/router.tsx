import { createBrowserRouter } from "react-router-dom";
import App from "./App";
import BookList from "./components/BookList";
import SingleBookDetails from "./components/SingleBookDetails";
import EditBook from "./components/EditBook";
import AddBook from "./components/AddBook";
import HomePage from "./components/HomePage";

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { path: "/", element: <HomePage /> },
      { path: "/books", element: <BookList /> },
      { path: "/books/new", element: <AddBook /> },
      { path: "/books/:id", element: <SingleBookDetails /> },
      { path: "/books/:id/edit", element: <EditBook /> },
    ],
  },
]);

export default router;
