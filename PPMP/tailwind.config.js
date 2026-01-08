module.exports = {
  mode: 'jit',
  darkMode: true, // or 'media' or 'class'
  theme: {
    extend: {
      screens: {
        'max-md': { 'max': '767px' },   // Apply below 768px
        'max-sm': { 'max': '639px' },   // Apply below 640px
      },


    },
  },
  variants: {
    extend: {
    },
  },
  plugins: [],
}
