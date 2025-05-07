import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import App from "./App";
import BookList from "./components/BookList";
import SingleBookDetails from "./components/SingleBookDetails";
import EditBook from "./components/EditBook";
import AddBook from "./components/AddBook";
import HomePage from "./components/HomePage";

const Router = () => (
  <BrowserRouter>
    <App />
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/books" element={<BookList />} />
      <Route path="/books/new" element={<AddBook />} />
      <Route path="/books/:id" element={<SingleBookDetails />} />
      <Route path="/books/:id/edit" element={<EditBook />} />
    </Routes>
  </BrowserRouter>
);

export default Router;
