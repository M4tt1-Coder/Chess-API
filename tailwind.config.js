/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './Pages/**/*.cshtml',
    './Views/**/*.cshtml'
  ],
  theme: {
    extend: {
      screens: {
        full_screen: {raw: '(min-height: 100vh)'},
      }
    },
  },
  plugins: [],
}

