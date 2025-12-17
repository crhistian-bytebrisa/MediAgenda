import {API_BASE_URL, AdaptResponse, getHeaders} from './base'

export const API = {
  
  GetAll: async (url) => {
    const response = await fetch(`${API_BASE_URL}/${url}`, {
      method: 'GET',
      headers: getHeaders(),
      credentials : 'include'
    });
    return AdaptResponse(response);
  },

  GetById: async (url,id) =>{
    const response = await fetch(`${API_BASE_URL}/${url}/${id}`, {
      method: 'GET',
      headers: getHeaders(),
      credentials : 'include'
    });
    return AdaptResponse(response);
  },

  Post: async (url, data) =>{
    const response = await fetch(`${API_BASE_URL}/${url}`, {
      method: 'POST',
      headers: getHeaders(),
      credentials : 'include',
      body: JSON.stringify(data)
    });
    return AdaptResponse(response);
  },

  Put: async (url, data) =>{
    const response = await fetch(`${API_BASE_URL}/${url}`, {
      method: 'PUT',
      headers: getHeaders(),
      credentials : 'include',
      body: JSON.stringify(data)
    });
    return AdaptResponse(response);
  },
  
  Delete: async (url) => {
    const response = await fetch(`${API_BASE_URL}/${url}`, {
      method: 'DELETE',
      headers: getHeaders(),
      credentials : 'include'
    });
    return AdaptResponse(response);
    }
}