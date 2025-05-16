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

  it("navigates to book details page when View is clicked", async () => {
    (bookService.fetchBooks as jest.Mock).mockResolvedValue(mockBooks);
    render(
      <BrowserRouter>
        <BookList />
      </BrowserRouter>
    );

    await screen.findByText("Book One");
    const viewButton = screen.getAllByRole("button", { name: /view/i })[0];
    fireEvent.click(viewButton);
    expect(window.location.pathname).toBe(`/books/1`);
  });

  it("navigates to edit book page when Edit is clicked", async () => {
    (bookService.fetchBooks as jest.Mock).mockResolvedValue(mockBooks);
    render(
      <BrowserRouter>
        <BookList />
      </BrowserRouter>
    );

    await screen.findByText("Book One");
    const editButton = screen.getAllByRole("button", { name: /edit/i })[0];
    fireEvent.click(editButton);
    expect(window.location.pathname).toBe(`/books/1/edit`);
  });

  it("navigates to home page when Go to Home Page is clicked", async () => {
    (bookService.fetchBooks as jest.Mock).mockResolvedValue(mockBooks);
    render(
      <BrowserRouter>
        <BookList />
      </BrowserRouter>
    );

    await screen.findByText("Book One");
    const homeButton = screen.getByRole("button", { name: /go to home page/i });
    fireEvent.click(homeButton);
    expect(window.location.pathname).toBe("/");
  });

  it("closes the delete modal when Cancel is clicked", async () => {
    (bookService.fetchBooks as jest.Mock).mockResolvedValue(mockBooks);
    render(
      <BrowserRouter>
        <BookList />
      </BrowserRouter>
    );

    await screen.findByText("Book One");
    fireEvent.click(screen.getAllByText(/delete/i)[0]);
    // The modal should be open now
    const cancelButton = screen.getByRole("button", { name: /no/i });
    fireEvent.click(cancelButton);
    // The modal should close, so the Cancel button should not be in the document
    await waitFor(() => {
      expect(
        screen.queryByRole("button", { name: /no/i })
      ).not.toBeInTheDocument();
    });
  });

  it("handles error when deleteBook fails", async () => {
    (bookService.fetchBooks as jest.Mock).mockResolvedValue(mockBooks);
    (bookService.deleteBook as jest.Mock).mockRejectedValue(
      new Error("Delete failed")
    );
    const consoleErrorSpy = jest
      .spyOn(console, "error")
      .mockImplementation(() => {});

    render(
      <BrowserRouter>
        <BookList />
      </BrowserRouter>
    );

    await screen.findByText("Book One");
    fireEvent.click(screen.getAllByText(/delete/i)[0]);
    fireEvent.click(screen.getByRole("button", { name: /confirm/i }));

    await waitFor(() => {
      expect(
        screen.queryByRole("button", { name: /confirm/i })
      ).not.toBeInTheDocument();
    });
    expect(screen.queryByText(/loading/i)).not.toBeInTheDocument();
    expect(consoleErrorSpy).toHaveBeenCalled();
    consoleErrorSpy.mockRestore();
  });

  it("handles error when fetchBooks fails in useEffect", async () => {
    (bookService.fetchBooks as jest.Mock).mockRejectedValue(
      new Error("Fetch failed")
    );
    const consoleErrorSpy = jest
      .spyOn(console, "error")
      .mockImplementation(() => {});

    render(
      <BrowserRouter>
        <BookList />
      </BrowserRouter>
    );

    // Wait for the loading spinner to disappear
    await waitFor(() => {
      expect(screen.queryByText(/loading/i)).not.toBeInTheDocument();
    });
    expect(consoleErrorSpy).toHaveBeenCalled();
    consoleErrorSpy.mockRestore();
  });
});
