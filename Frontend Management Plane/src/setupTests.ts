/**
 * @file This file is executed before each test file, setting up the global test environment.
 * It extends the `expect` function from Vitest with custom matchers from
 * `@testing-library/jest-dom` to make DOM assertions more expressive.
 */

import { expect, afterEach } from 'vitest';
import { cleanup } from '@testing-library/react';
import * as matchers from '@testing-library/jest-dom/matchers';

// Extends Vitest's expect method with methods from react-testing-library
expect.extend(matchers);

// Runs a cleanup after each test case (e.g., clearing jsdom)
afterEach(() => {
  cleanup();
});