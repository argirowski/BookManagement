import React from "react";
import { render, screen } from "@testing-library/react";
import LoadingSpinner from "../components/loader/LoadingSpinner";

describe("LoadingSpinner Component", () => {
  test("renders a spinner with the correct role and visually hidden text", () => {
    render(<LoadingSpinner />);

    const spinner = screen.getByRole("status");
    expect(spinner).toBeInTheDocument();
    expect(spinner).toHaveClass("spinner-border");

    const visuallyHiddenText = screen.getByText("Loading...");
    expect(visuallyHiddenText).toBeInTheDocument();
    expect(visuallyHiddenText).toHaveClass("visually-hidden");
  });
});
