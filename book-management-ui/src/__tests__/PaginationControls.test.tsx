import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import PaginationControls from "../components/common/PaginationControls";

describe("PaginationControls", () => {
  it("renders the correct number of pages and highlights the active page", () => {
    render(
      <PaginationControls page={2} totalPages={4} onPageChange={() => {}} />
    );
    expect(screen.getByText("1")).toBeInTheDocument();
    expect(screen.getByText("2")).toBeInTheDocument();
    expect(screen.getByText("3")).toBeInTheDocument();
    expect(screen.getByText("4")).toBeInTheDocument();
    // Find the active <li> by role and class, and check it contains "2"
    const activeItem = screen.getAllByRole("listitem").find(li => li.classList.contains("active"));
    expect(activeItem).toBeInTheDocument();
    expect(activeItem).toHaveTextContent("2");
  });

  it("calls onPageChange with the correct page number when a page is clicked", () => {
    const onPageChange = jest.fn();
    render(
      <PaginationControls page={1} totalPages={3} onPageChange={onPageChange} />
    );
    fireEvent.click(screen.getByText("3"));
    expect(onPageChange).toHaveBeenCalledWith(3);
  });

  it("renders nothing if totalPages is 0", () => {
    render(
      <PaginationControls page={1} totalPages={0} onPageChange={() => {}} />
    );
    // Should not render any page items
    expect(screen.queryByRole("button")).not.toBeInTheDocument();
  });
});
