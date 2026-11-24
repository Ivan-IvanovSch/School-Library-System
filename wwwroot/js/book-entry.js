document.querySelectorAll('[data-book-entry]').forEach(form => {
 const input = form.querySelector('[data-cover-input]');
 const image = form.querySelector('[data-cover-image]');
 const placeholder = form.querySelector('[data-cover-placeholder]');
 const error = form.querySelector('[data-cover-error]');
 let url;
 input.addEventListener('change', () => {
  const file = input.files[0];
  error.textContent = ''; input.setCustomValidity('');
  if (!file) return;
  if (!['image/jpeg','image/png','image/webp'].includes(file.type) || file.size > 5 * 1024 * 1024) {
   error.textContent = 'Choose a JPG, PNG or WebP image under 5 MB.';
   input.setCustomValidity(error.textContent); return;
  }
  if(url) URL.revokeObjectURL(url);
  url = URL.createObjectURL(file); image.src = url; image.hidden = false; placeholder.hidden = true;
 });
});
