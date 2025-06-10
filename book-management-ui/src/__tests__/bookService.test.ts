import axios from "axios";
import * as bookService from "../services/bookService";

jest.mock("axios");
const mockedAxios = axios as jest.Mocked<typeof axios>;

const mockBook = {
  bookId: "1",
  title: "Book One",
  author: "Author One",
  publishedDate: "2023-01-01",
  categoryNames: ["Fiction", "Adventure"],
};

describe("bookService", () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it("fetchBooks returns paged data", async () => {
    const pagedResult = {
      items: [mockBook],
      totalPages: 3,
    };
    mockedAxios.get.mockResolvedValueOnce({ data: pagedResult });
    const result = await bookService.fetchBooks(1, 5);
    expect(result).toEqual(pagedResult);
    expect(mockedAxios.get).toHaveBeenCalledWith(
      "https://localhost:7272/api/books",
      { params: { pageNumber: 1, pageSize: 5 } }
    );
  });

  it("fetchBookById returns data", async () => {
    mockedAxios.get.mockResolvedValueOnce({ data: mockBook });
    const result = await bookService.fetchBookById("1");
    expect(result).toEqual(mockBook);
    expect(mockedAxios.get).toHaveBeenCalledWith(
      "https://localhost:7272/api/books/1"
    );
  });

  it("deleteBook calls axios.delete", async () => {
    mockedAxios.delete.mockResolvedValueOnce({});
    await bookService.deleteBook("1");
    expect(mockedAxios.delete).toHaveBeenCalledWith(
      "https://localhost:7272/api/books/1"
    );
  });

  it("addBook posts data and returns response", async () => {
    mockedAxios.post.mockResolvedValueOnce({ data: mockBook });
    const result = await bookService.addBook(mockBook);
    expect(result).toEqual(mockBook);
    expect(mockedAxios.post).toHaveBeenCalledWith(
      "https://localhost:7272/api/books",
      mockBook
    );
  });

  it("updateBook puts data and returns response", async () => {
    mockedAxios.put.mockResolvedValueOnce({ data: mockBook });
    const result = await bookService.updateBook(mockBook);
    expect(result).toEqual(mockBook);
    expect(mockedAxios.put).toHaveBeenCalledWith(
      `https://localhost:7272/api/books/${mockBook.bookId}`,
      mockBook
    );
  });

  it("logs and throws on fetchBooks error", async () => {
    const error = new Error("fail");
    mockedAxios.get.mockRejectedValueOnce(error);
    const consoleSpy = jest
      .spyOn(console, "error")
      .mockImplementation(() => {});
    await expect(bookService.fetchBooks()).rejects.toThrow(error);
    expect(consoleSpy).toHaveBeenCalledWith("Error fetching books:", error);
    consoleSpy.mockRestore();
  });

  it("logs and throws on fetchBookById error", async () => {
    const error = new Error("fail");
    mockedAxios.get.mockRejectedValueOnce(error);
    const consoleSpy = jest
      .spyOn(console, "error")
      .mockImplementation(() => {});
    await expect(bookService.fetchBookById("1")).rejects.toThrow(error);
    expect(consoleSpy).toHaveBeenCalledWith("Error fetching book:", error);
    consoleSpy.mockRestore();
  });

  it("logs and throws on deleteBook error", async () => {
    const error = new Error("fail");
    mockedAxios.delete.mockRejectedValueOnce(error);
    const consoleSpy = jest
      .spyOn(console, "error")
      .mockImplementation(() => {});
    await expect(bookService.deleteBook("1")).rejects.toThrow(error);
    expect(consoleSpy).toHaveBeenCalledWith("Error deleting book:", error);
    consoleSpy.mockRestore();
  });

  it("logs and throws on addBook error", async () => {
    const error = new Error("fail");
    mockedAxios.post.mockRejectedValueOnce(error);
    const consoleSpy = jest
      .spyOn(console, "error")
      .mockImplementation(() => {});
    await expect(bookService.addBook(mockBook)).rejects.toThrow(error);
    expect(consoleSpy).toHaveBeenCalledWith("Error adding book:", error);
    consoleSpy.mockRestore();
  });

  it("logs and throws on updateBook error", async () => {
    const error = new Error("fail");
    mockedAxios.put.mockRejectedValueOnce(error);
    const consoleSpy = jest
      .spyOn(console, "error")
      .mockImplementation(() => {});
    await expect(bookService.updateBook(mockBook)).rejects.toThrow(error);
    expect(consoleSpy).toHaveBeenCalledWith("Error updating book:", error);
    consoleSpy.mockRestore();
  });
});
