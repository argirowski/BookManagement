import {
  formatDate,
  handleGoBack,
  handleModalClose,
  handleModalConfirm,
} from "../../utils/formUtils";

describe("formatDate", () => {
  it("formats a valid date string to YYYY-MM-DD", () => {
    expect(formatDate("2023-05-16")).toBe("2023-05-16");
    expect(formatDate("1999-01-01")).toBe("1999-01-01");
    expect(formatDate("2025-12-31")).toBe("2025-12-31");
  });

  it("pads single-digit months and days with zeros", () => {
    expect(formatDate("2023-1-5")).toBe("2023-01-05");
    expect(formatDate("2023-2-9")).toBe("2023-02-09");
    expect(formatDate("2023-11-3")).toBe("2023-11-03");
  });

  it("handles ISO date strings with time", () => {
    expect(formatDate("2023-05-16T12:34:56Z")).toBe("2023-05-16");
  });

  it("returns 'Invalid Date' for invalid input", () => {
    expect(formatDate("")).toBe("NaN-NaN-NaN");
    expect(formatDate("not-a-date")).toBe("NaN-NaN-NaN");
  });
});

describe("handleGoBack", () => {
  it("shows the modal if the form is dirty", () => {
    const setShowModal = jest.fn();
    const navigate = jest.fn();
    const isFormDirty = true;
    const goBack = handleGoBack(isFormDirty, setShowModal, navigate);
    goBack();
    expect(setShowModal).toHaveBeenCalledWith(true);
    expect(navigate).not.toHaveBeenCalled();
  });

  it("navigates back if the form is not dirty", () => {
    const setShowModal = jest.fn();
    const navigate = jest.fn();
    const isFormDirty = false;
    const goBack = handleGoBack(isFormDirty, setShowModal, navigate);
    goBack();
    expect(setShowModal).not.toHaveBeenCalled();
    expect(navigate).toHaveBeenCalledWith(-1);
  });
});

describe("handleModalClose", () => {
  it("sets showModal to false when called", () => {
    const setShowModal = jest.fn();
    const close = handleModalClose(setShowModal);
    close();
    expect(setShowModal).toHaveBeenCalledWith(false);
  });
});

describe("handleModalConfirm", () => {
  it("sets showModal to false and navigates back when called", () => {
    const setShowModal = jest.fn();
    const navigate = jest.fn();
    const confirm = handleModalConfirm(setShowModal, navigate);
    confirm();
    expect(setShowModal).toHaveBeenCalledWith(false);
    expect(navigate).toHaveBeenCalledWith(-1);
  });
});
