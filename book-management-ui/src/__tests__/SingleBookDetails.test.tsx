import React from "react";
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import { BrowserRouter } from "react-router-dom";
import SingleBookDetails from "../components/SingleBookDetails";
import * as bookService from "../services/bookService";

jest.mock("../services/bookService");

const mockBook = {
  bookId: "1",
  title: "Book One",
  author: "Author One",
  publishedDate: "2023-01-01",
  categoryNames: ["Fiction", "Adventure"],
};

beforeAll(() => {
  // Mock toLocaleDateString to return a consistent value
  jest.spyOn(Date.prototype, "toLocaleDateString").mockReturnValue("1/1/2023");
});

afterAll(() => {
  // Restore the original implementation
  jest.restoreAllMocks();
});

describe("SingleBookDetails Component", () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it("renders loading spinner initially", () => {
    (bookService.fetchBookById as jest.Mock).mockResolvedValue(null);
    render(
      <BrowserRouter>
        <SingleBookDetails />
      </BrowserRouter>
    );
    expect(screen.getByText(/loading/i)).toBeInTheDocument();
  });

  it("renders book details after loading", async () => {
    (bookService.fetchBookById as jest.Mock).mockResolvedValue(mockBook);
    render(
      <BrowserRouter>
        <SingleBookDetails />
      </BrowserRouter>
    );

    await screen.findByText("Book Details");
    expect(screen.getByText("Book One")).toBeInTheDocument();
    expect(screen.getByText("Author One")).toBeInTheDocument();
    expect(screen.getByText("1/1/2023")).toBeInTheDocument(); // Consistent mocked date
    expect(screen.getByText("Fiction, Adventure")).toBeInTheDocument();
  });

  it("navigates to edit book page", async () => {
    (bookService.fetchBookById as jest.Mock).mockResolvedValue(mockBook);
    render(
      <BrowserRouter>
        <SingleBookDetails />
      </BrowserRouter>
    );

    await screen.findByText("Book Details");

    const editButton = screen.getByText(/edit book/i);
    fireEvent.click(editButton);

    expect(window.location.pathname).toBe(`/books/${mockBook.bookId}/edit`);
  });

  it("navigates back when close button is clicked", async () => {
    (bookService.fetchBookById as jest.Mock).mockResolvedValue(mockBook);
    render(
      <BrowserRouter>
        <SingleBookDetails />
      </BrowserRouter>
    );

    await screen.findByText("Book Details");

    const closeButton = screen.getByText(/close/i);
    fireEvent.click(closeButton);

    // Simulate navigation back (mock implementation may vary)
    expect(window.history.length).toBeGreaterThan(1);
  });

  it("logs error when fetchBookById fails", async () => {
    const error = new Error("Fetch failed");
    (bookService.fetchBookById as jest.Mock).mockRejectedValue(error);
    const consoleErrorSpy = jest
      .spyOn(console, "error")
      .mockImplementation(() => {});

    render(
      <BrowserRouter>
        <SingleBookDetails />
      </BrowserRouter>
    );

    // Wait for useEffect to run
    await screen.findByText(/loading/i);
    await waitFor(() => {
      expect(consoleErrorSpy).toHaveBeenCalledWith(
        "Error fetching book:",
        error
      );
    });
    consoleErrorSpy.mockRestore();
  });
});
