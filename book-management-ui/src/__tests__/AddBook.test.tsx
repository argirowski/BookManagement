import React from "react";
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { BrowserRouter } from "react-router-dom";
import AddBook from "../components/AddBook";

// jest.mock calls must be at the very top for proper mocking
jest.mock("../services/bookService");
jest.mock("react-router-dom", () => ({
  ...jest.requireActual("react-router-dom"),
  useNavigate: () => jest.fn(),
}));

describe("AddBook Component", () => {
  let addBookMock: jest.Mock;
  let mockNavigate: jest.Mock;

  beforeEach(() => {
    jest.clearAllMocks();
    addBookMock = require("../services/bookService").addBook;
    mockNavigate = require("react-router-dom").useNavigate();
  });

  it("renders the form fields correctly", () => {
    render(
      <BrowserRouter>
        <AddBook />
      </BrowserRouter>
    );

    expect(screen.getByText(/Title/i)).toBeInTheDocument();
    expect(screen.getByText(/Author/i)).toBeInTheDocument();
    expect(screen.getByText(/Published Date/i)).toBeInTheDocument();
    expect(screen.getByText(/Categories/i)).toBeInTheDocument();
    expect(
      screen.getByRole("button", { name: /add book/i })
    ).toBeInTheDocument();
    expect(
      screen.getByRole("button", { name: /go back/i })
    ).toBeInTheDocument();
  });

  it("validates form fields", async () => {
    render(
      <BrowserRouter>
        <AddBook />
      </BrowserRouter>
    );

    fireEvent.click(screen.getByRole("button", { name: /add book/i }));

    expect(await screen.findByText(/Title is required/i)).toBeInTheDocument();
    expect(await screen.findByText(/Author is required/i)).toBeInTheDocument();
    expect(
      await screen.findByText(/Published Date is required/i)
    ).toBeInTheDocument();
    expect(
      await screen.findByText(/At least one category is required/i)
    ).toBeInTheDocument();
  });

  it("submits the form with valid data and calls addBook", async () => {
    addBookMock.mockResolvedValueOnce({ id: 1 });

    render(
      <BrowserRouter>
        <AddBook />
      </BrowserRouter>
    );

    // Fill in the form
    fireEvent.change(screen.getByLabelText(/Title/i), {
      target: { value: "Test Book" },
    });
    fireEvent.change(screen.getByLabelText(/Author/i), {
      target: { value: "Test Author" },
    });
    fireEvent.change(screen.getByLabelText(/Published Date/i), {
      target: { value: "2023-05-16" },
    });

    // Use userEvent to select a category (e.g., Technology)
    const selectInput = screen.getByRole("combobox", { name: /Categories/i });
    fireEvent.keyDown(selectInput, { key: "ArrowDown" });
    const option = await screen.findByText("Technology");
    userEvent.click(option);

    // Submit the form
    fireEvent.click(screen.getByRole("button", { name: /Add Book/i }));

    await waitFor(() => {
      expect(addBookMock).toHaveBeenCalledWith({
        title: "Test Book",
        author: "Test Author",
        publishedDate: "2023-05-16",
        categoryNames: ["Technology"],
      });
    });
  });
});
