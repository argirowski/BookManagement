import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import { BrowserRouter } from "react-router-dom";
import AddBook from "../components/AddBook";

jest.mock("../services/bookService");

describe("AddBook Component", () => {
  beforeEach(() => {
    jest.clearAllMocks();
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
});
