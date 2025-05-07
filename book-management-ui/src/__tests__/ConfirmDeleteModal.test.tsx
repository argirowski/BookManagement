import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import ConfirmDeleteModal from "../components/modals/ConfirmDeleteModal";

describe("ConfirmDeleteModal Component", () => {
  const mockOnHide = jest.fn();
  const mockOnConfirm = jest.fn();

  test("renders with correct title and body text", () => {
    render(
      <ConfirmDeleteModal
        show={true}
        onHide={mockOnHide}
        onConfirm={mockOnConfirm}
      />
    );

    expect(screen.getByText("Confirm Delete")).toBeInTheDocument();
    expect(
      screen.getByText("Are you sure you want to delete this item?")
    ).toBeInTheDocument();
  });

  test("calls onConfirm when 'Yes' button is clicked", () => {
    render(
      <ConfirmDeleteModal
        show={true}
        onHide={mockOnHide}
        onConfirm={mockOnConfirm}
      />
    );

    const yesButton = screen.getByText("Yes");
    fireEvent.click(yesButton);

    expect(mockOnConfirm).toHaveBeenCalledTimes(1);
  });

  test("calls onHide when 'No' button is clicked", () => {
    render(
      <ConfirmDeleteModal
        show={true}
        onHide={mockOnHide}
        onConfirm={mockOnConfirm}
      />
    );

    const noButton = screen.getByText("No");
    fireEvent.click(noButton);

    expect(mockOnHide).toHaveBeenCalledTimes(1);
  });
});
