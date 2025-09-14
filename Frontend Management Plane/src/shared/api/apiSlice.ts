import { createApi } from '@reduxjs/toolkit/query/react';
import type { BaseQueryFn } from '@reduxjs/toolkit/query/react';
import type { AxiosRequestConfig, AxiosError } from 'axios';
import axiosInstance from './axiosInstance';

/**
 * A custom BaseQueryFn implementation that wraps our configured Axios instance.
 * This allows RTK Query to use our centralized Axios configuration, including
 * interceptors for authentication (attaching JWT) and global error handling.
 *
 * It transforms Axios errors into a format that RTK Query can understand
 * and expose through its hooks (`isError`, `error`).
 */
const axiosBaseQuery: BaseQueryFn<
  {
    url: string;
    method?: AxiosRequestConfig['method'];
    data?: AxiosRequestConfig['data'];
    params?: AxiosRequestConfig['params'];
    headers?: AxiosRequestConfig['headers'];
  },
  unknown,
  {
    status?: number;
    data?: unknown;
    message?: string;
  }
> = async ({ url, method = 'GET', data, params, headers }) => {
  try {
    const result = await axiosInstance({
      url,
      method,
      data,
      params,
      headers,
    });
    return { data: result.data };
  } catch (axiosError) {
    const err = axiosError as AxiosError;
    return {
      error: {
        status: err.response?.status,
        data: err.response?.data,
        message: err.message,
      },
    };
  }
};

/**
 * The root API slice for the application.
 *
 * This slice defines the base configuration for all our interactions with the backend API.
 * Feature-specific API endpoints are "injected" into this slice in their respective
 * feature folders. This creates a single, centralized API slice with a shared cache
 * and middleware, which is the recommended pattern for scalability with RTK Query.
 *
 * `tagTypes` are defined here to establish a central registry of cache tags that can be
 * provided and invalidated by different endpoints, enabling automated data re-fetching.
 *
 * @see https://redux-toolkit.js.org/rtk-query/api/createApi
 * @see https://redux-toolkit.js.org/rtk-query/usage/code-splitting
 */
export const apiSlice = createApi({
  reducerPath: 'api',
  baseQuery: axiosBaseQuery,
  tagTypes: [
    // Core Entities based on requirements analysis
    'User',
    'Role',
    'Tenant',
    'Asset',
    'AssetTemplate',
    'OpcTag',
    'OpcClient',
    'OpcServerConnection',
    'Alarm',
    'AuditLog',
    'AiModel',
    'AiModelVersion',
    'ReportTemplate',
    'ReportSchedule',
    'Dashboard',
    'License',
    'NotificationPreference',
    'Integration',
    'ArMapping',
    'DataImportJob',
  ],
  endpoints: () => ({}), // Endpoints will be injected from feature slices
});

export default apiSlice;