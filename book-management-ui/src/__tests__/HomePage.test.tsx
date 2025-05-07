import React from "react";
import { render, screen } from "@testing-library/react";
import { BrowserRouter } from "react-router-dom";
import HomePage from "../components/HomePage";

describe("HomePage Component", () => {
  test("renders welcome message", () => {
    render(
      <BrowserRouter>
        <HomePage />
      </BrowserRouter>
    );

    expect(screen.getByTestId("welcome-message")).toBeInTheDocument();
  });

  test("renders link to add a new book", () => {
    render(
      <BrowserRouter>
        <HomePage />
      </BrowserRouter>
    );

    const addBookLink = screen.getByTestId("add-book-link");
    expect(addBookLink).toBeInTheDocument();
    expect(addBookLink).toHaveAttribute("href", "/books/new");
  });

  test("renders link to view book selection", () => {
    render(
      <BrowserRouter>
        <HomePage />
      </BrowserRouter>
    );

    const viewBooksLink = screen.getByTestId("view-books-link");
    expect(viewBooksLink).toBeInTheDocument();
    expect(viewBooksLink).toHaveAttribute("href", "/books");
  });
});
