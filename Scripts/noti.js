const noties = document.querySelectorAll(".noti")
const notiCloses = document.querySelectorAll('.noti-close')
noties.forEach(noti => {
    noti.addEventListener('click', () => {
        noti.style.display = 'none'
    })
})
notiCloses.forEach(notiClose => {
    notiClose.addEventListener('click', () => {
        noti.style.display = 'none'
    })
})