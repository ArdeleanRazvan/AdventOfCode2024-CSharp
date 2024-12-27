# Explanation of Linear Equations

This snippet represents a system of linear equations, often encountered in vector algebra or geometry, and solves for
coefficients \(a\) and \(b\) in terms of given parameters.

## System of Equations

You start with two linear equations:

1. $$ a \cdot ax + b \cdot bx = px $$
2. $$ a \cdot ay + b \cdot by = py $$

These equations represent a combination of two vectors, \((ax, ay)\) and \((bx, by)\), being scaled by \(a\) and \(b\),
respectively, to produce the resultant vector \((px, py)\).

## Goal

Solve for \(a\) and \(b\), which are the scalar coefficients.

---

## Determinant (\(D\))

The determinant \(D\) is a scalar value that helps check whether the given vectors \((ax, ay)\) and \((bx, by)\) are
linearly independent (i.e., not parallel). It is calculated as:
$$
D = ax \cdot by - ay \cdot bx
$$

- If \(D = 0\), the vectors are parallel or collinear, and the system does not have a unique solution.

---

## Solving for \(a\) and \(b\)

Using the determinant, the equations are solved as follows:

1. Solve for \(a\):
   $$
   a = \frac{(px \cdot by - py \cdot bx)}{D}
   $$
   This formula ensures the scalar \(a\) is determined based on the projection of \((px, py)\) along the direction
   of \((ax, ay)\).

2. Solve for \(b\):
   $$
   b = \frac{(ax \cdot py - ay \cdot px)}{D}
   $$
   This formula determines the scalar \(b\) by isolating the contribution of \((bx, by)\).

---

## Summary

- This system decomposes a vector \((px, py)\) into a linear combination of two given vectors \((ax, ay)\) and \((bx,
  by)\).
- \(D\) ensures the vectors are not collinear.
- The derived formulas for \(a\) and \(b\) give unique solutions if \(D \neq 0\).

---

## Example

Suppose:

- \((ax, ay) = (2, 3)\)
- \((bx, by) = (1, -1)\)
- \((px, py) = (5, 2)\)

### Step 1: Calculate \(D\)

$$
D = 2 \cdot (-1) - 3 \cdot 1 = -2 - 3 = -5
$$

### Step 2: Find \(a\)

$$
a = \frac{(5 \cdot -1 - 2 \cdot 1)}{-5} = \frac{-5 - 2}{-5} = \frac{-7}{-5} = 1.4
$$

### Step 3: Find \(b\)

$$
b = \frac{(2 \cdot 2 - 3 \cdot 5)}{-5} = \frac{4 - 15}{-5} = \frac{-11}{-5} = 2.2
$$

Thus, the solution is \(a = 1.4\) and \(b = 2.2\).
