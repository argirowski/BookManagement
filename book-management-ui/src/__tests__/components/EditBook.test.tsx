import React from "react";
import { render, screen, waitFor, fireEvent } from "@testing-library/react";
import { BrowserRouter } from "react-router-dom";
import * as bookService from "../../services/bookService";
import EditBook from "../../components/EditBook";

jest.mock("../../services/bookService");
jest.mock("react-router-dom", () => ({
  ...jest.requireActual("react-router-dom"),
  useParams: () => ({ id: "1" }),
  useNavigate: () => jest.fn(),
}));

const mockBook = {
  bookId: "1",
  title: "Book One",
  author: "Author One",
  publishedDate: "2023-01-01",
  categoryNames: ["Fiction", "Adventure"],
};

describe("EditBook Component", () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it("shows loading spinner while fetching", async () => {
    (bookService.fetchBookById as jest.Mock).mockImplementation(
      () => new Promise(() => {})
    );
    render(
      <BrowserRouter>
        <EditBook />
      </BrowserRouter>
    );
    expect(screen.getByText(/loading/i)).toBeInTheDocument();
  });

  it("renders book data after loading", async () => {
    (bookService.fetchBookById as jest.Mock).mockResolvedValue(mockBook);
    render(
      <BrowserRouter>
        <EditBook />
      </BrowserRouter>
    );
    expect(await screen.findByDisplayValue("Book One")).toBeInTheDocument();
    expect(screen.getByDisplayValue("Author One")).toBeInTheDocument();
    expect(screen.getByDisplayValue("2023-01-01")).toBeInTheDocument();
    expect(screen.getByText("Fiction")).toBeInTheDocument();
    // expect(screen.getByText("Adventure")).toBeInTheDocument();
  });

  it("submits the form and calls updateBook", async () => {
    (bookService.fetchBookById as jest.Mock).mockResolvedValue(mockBook);
    (bookService.updateBook as jest.Mock).mockResolvedValue({});

    render(
      <BrowserRouter>
        <EditBook />
      </BrowserRouter>
    );

    // Wait for form to load
    await screen.findByDisplayValue("Book One");

    // Change title
    fireEvent.change(screen.getByDisplayValue("Book One"), {
      target: { value: "Updated Book" },
    });

    // Submit
    fireEvent.click(screen.getByRole("button", { name: /save changes/i }));

    await waitFor(() => {
      expect(bookService.updateBook).toHaveBeenCalledWith(
        expect.objectContaining({
          title: "Updated Book",
          author: "Author One",
          publishedDate: "2023-01-01",
          categoryNames: ["Fiction", "Adventure"],
        })
      );
    });
  });

  it("shows validation errors if required fields are empty", async () => {
    (bookService.fetchBookById as jest.Mock).mockResolvedValue(mockBook);

    render(
      <BrowserRouter>
        <EditBook />
      </BrowserRouter>
    );

    await screen.findByDisplayValue("Book One");

    // Clear title
    fireEvent.change(screen.getByDisplayValue("Book One"), {
      target: { value: "" },
    });

    fireEvent.click(screen.getByRole("button", { name: /save changes/i }));

    expect(await screen.findByText(/title is required/i)).toBeInTheDocument();
  });

  it("shows modal when navigating back with unsaved changes", async () => {
    (bookService.fetchBookById as jest.Mock).mockResolvedValue(mockBook);

    render(
      <BrowserRouter>
        <EditBook />
      </BrowserRouter>
    );

    await screen.findByDisplayValue("Book One");

    // Change title to make form dirty
    fireEvent.change(screen.getByDisplayValue("Book One"), {
      target: { value: "Dirty Book" },
    });

    // Click Go Back
    fireEvent.click(screen.getByRole("button", { name: /go back/i }));

    expect(
      await screen.findByText(/do you really want to go back\? you will lose all of your changes\./i)
    ).toBeInTheDocument();
  });

  it("logs error if fetchBookById fails", async () => {
    const error = new Error("Fetch failed");
    (bookService.fetchBookById as jest.Mock).mockRejectedValue(error);
    const consoleErrorSpy = jest
      .spyOn(console, "error")
      .mockImplementation(() => {});

    render(
      <BrowserRouter>
        <EditBook />
      </BrowserRouter>
    );

    await waitFor(() => {
      expect(consoleErrorSpy).toHaveBeenCalledWith(
        "Error fetching book:",
        error
      );
    });

    consoleErrorSpy.mockRestore();
  });

  it("logs error if updateBook fails", async () => {
    (bookService.fetchBookById as jest.Mock).mockResolvedValue(mockBook);
    (bookService.updateBook as jest.Mock).mockRejectedValue(
      new Error("Update failed")
    );
    const consoleErrorSpy = jest
      .spyOn(console, "error")
      .mockImplementation(() => {});

    render(
      <BrowserRouter>
        <EditBook />
      </BrowserRouter>
    );

    await screen.findByDisplayValue("Book One");

    fireEvent.click(screen.getByRole("button", { name: /save changes/i }));

    await waitFor(() => {
      expect(consoleErrorSpy).toHaveBeenCalledWith(
        "Error editing book:",
        expect.any(Error)
      );
    });

    consoleErrorSpy.mockRestore();
  });
});
