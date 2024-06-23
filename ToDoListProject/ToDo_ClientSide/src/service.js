import axios from 'axios';


const apiUrl = process.env.REACT_APP_API_URL;
axios.defaults.baseURL = apiUrl;

axios.interceptors.response.use(
  response => response,
  error => {
    console.error('Error:', error);
    return Promise.reject(error);
  }
);

export default {

  getTasks: async () => {
    const result = await axios.get(`${apiUrl}/items`)
    console.log(result.data);
    return result.json;
  },

  addTask: async (name) => {
    try {
      console.log("Adding task:", name);
      const newItem = {name: name, isComplete: false }; // Constructing the task object
      const result = await axios.post(`${apiUrl}/items`, newItem); // Sending POST request with the task data
      return result.data; // Returning the data from the response, if needed
    } catch (err) {
      console.error('Error adding task:', err);
      throw err; // Rethrow the error for further handling
    }
  },

  setCompleted: async (id,taskName, isComplete) => {
    try {
      
      const updatedItem = {name: taskName, isComplete: isComplete }; // Constructing the updated task object
      const result = await axios.put(`${apiUrl}/items/${id}`, updatedItem); // Sending PUT request with the updated task data
      console.log('Setting completion status for task:', id, isComplete);
      return result.data; // Returning the data from the response, if needed
    } catch (err) {
      console.error('Error setting completion status for task:', err);
      throw err; // Rethrow the error for further handling
    }
  },

  deleteTask: async (id) => {
    
    try {
      const result = await axios.delete(`${apiUrl}/items/${id}`);
      console.log('deleteTask');
      return result.data; // You may or may not return data after successful deletion
    } catch (err) {
      console.error('Error deleting task:', err);
      throw err;
    }  }
};
