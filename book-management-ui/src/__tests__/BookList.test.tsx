import React from "react";
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import { BrowserRouter } from "react-router-dom";
import BookList from "../components/BookList";
import * as bookService from "../services/bookService";

jest.mock("../services/bookService");

const mockBooks = [
  {
    bookId: "1",
    title: "Book One",
    author: "Author One",
    publishedDate: "2023-01-01",
    categoryNames: ["Fiction", "Adventure"],
  },
  {
    bookId: "2",
    title: "Book Two",
    author: "Author Two",
    publishedDate: "2022-05-15",
    categoryNames: ["Non-Fiction"],
  },
];

describe("BookList Component", () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it("renders loading spinner initially", () => {
    (bookService.fetchBooks as jest.Mock).mockResolvedValue([]);
    render(
      <BrowserRouter>
        <BookList />
      </BrowserRouter>
    );
    expect(screen.getByText(/loading/i)).toBeInTheDocument();
  });

  it("renders books after loading", async () => {
    (bookService.fetchBooks as jest.Mock).mockResolvedValue(mockBooks);
    render(
      <BrowserRouter>
        <BookList />
      </BrowserRouter>
    );

    await screen.findByText("Book One");
    expect(screen.getByText("Author One")).toBeInTheDocument();
    expect(screen.getByText("Fiction, Adventure")).toBeInTheDocument();
  });

  it("handles delete action", async () => {
    (bookService.fetchBooks as jest.Mock).mockResolvedValue(mockBooks);
    (bookService.deleteBook as jest.Mock).mockResolvedValue({});

    render(
      <BrowserRouter>
        <BookList />
      </BrowserRouter>
    );

    await screen.findByText("Book One");

    fireEvent.click(screen.getAllByText(/delete/i)[0]);
    fireEvent.click(screen.getByRole("button", { name: /confirm/i }));

    await waitFor(() =>
      expect(bookService.deleteBook).toHaveBeenCalledWith("1")
    );
  });

  it("navigates to add new book page", async () => {
    (bookService.fetchBooks as jest.Mock).mockResolvedValue(mockBooks);
    render(
      <BrowserRouter>
        <BookList />
      </BrowserRouter>
    );

    // Ensure the component renders completely before interacting with the button
    await screen.findByText("Book One");

    const addButton = screen.getByTestId("add-new-book-button");
    fireEvent.click(addButton);

    expect(window.location.pathname).toBe("/books/new");
  });
});
