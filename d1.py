import cv2
import numpy as np

image = cv2.imread("D:/113.jpg")

gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
gray = cv2.GaussianBlur(gray, (3, 3), 0)
cv2.imwrite("gray.jpg", gray)


edged = cv2.Canny(gray, 10, 250)
cv2.imwrite("edged.jpg", edged)


kernel = cv2.getStructuringElement(cv2.MORPH_RECT, (7, 7))
closed = cv2.morphologyEx(edged, cv2.MORPH_CLOSE, kernel)

cv2.imwrite("closed.jpg", closed)


cnts = cv2.findContours(closed.copy(), cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)[1]

cv2.drawContours(image, cnts, -1, (0, 255, 0), 2)
cv2.imshow("image.jpg", image)
cv2.waitKey(0)

