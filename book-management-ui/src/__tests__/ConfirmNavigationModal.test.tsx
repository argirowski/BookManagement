import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import ConfirmNavigationModal from "../components/modals/ConfirmNavigationModal";

describe("ConfirmNavigationModal Component", () => {
  const mockOnHide = jest.fn();
  const mockOnConfirm = jest.fn();

  test("renders with correct title and body text", () => {
    render(
      <ConfirmNavigationModal
        show={true}
        onHide={mockOnHide}
        onConfirm={mockOnConfirm}
      />
    );

    expect(screen.getByText("Confirm Navigation")).toBeInTheDocument();
    expect(
      screen.getByText(
        "Do you really want to go back? You will lose all of your changes."
      )
    ).toBeInTheDocument();
  });

  test("calls onConfirm when 'Yes' button is clicked", () => {
    render(
      <ConfirmNavigationModal
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
      <ConfirmNavigationModal
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
